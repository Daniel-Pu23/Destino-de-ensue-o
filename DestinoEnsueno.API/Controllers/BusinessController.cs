using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly IBusinessService _businessService;

    private static readonly HttpClient _geoHttpClient = CreateGeoHttpClient();
    private static readonly SemaphoreSlim _rateLimitSemaphore = new(1, 1);
    private static DateTime _lastNominatimRequestUtc = DateTime.MinValue;
    private static readonly ConcurrentDictionary<string, string> _geoCache = new();

    private static HttpClient CreateGeoHttpClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("DestinoDeEnsuenoApp/1.0 (contacto@destino.com)");
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("es,en;q=0.8");
        return client;
    }

    public BusinessController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId)
    {
        return Ok(await _businessService.GetAllPublishedAsync(search, categoryId));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllForAdmin()
    {
        return Ok(await _businessService.GetAllForAdminAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var b = await _businessService.GetByIdAsync(id);
        if (b == null) return NotFound(new { message = "Negocio no encontrado." });
        return Ok(b);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BusinessCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var b = await _businessService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = b.Id }, b);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BusinessCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updated = await _businessService.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { message = "El negocio que intenta editar no existe." });
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _businessService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "El negocio que intenta eliminar no existe." });
        return Ok(new { message = "Negocio eliminado correctamente." });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("geocode")]
    public async Task<IActionResult> Geocode([FromQuery] string? q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
            return BadRequest(new { message = "Ingresa al menos 2 caracteres para buscar una dirección o lugar." });

        var query = q.Trim();
        var cacheKey = $"search:{query.ToLowerInvariant()}";

        try
        {
            if (!_geoCache.TryGetValue(cacheKey, out var rawJson))
            {
                var url = $"https://nominatim.openstreetmap.org/search?format=jsonv2&addressdetails=1&limit=5&q={Uri.EscapeDataString(query)}";
                rawJson = await FetchFromNominatimAsync(url);
                _geoCache[cacheKey] = rawJson;
            }

            using var doc = JsonDocument.Parse(rawJson);
            var results = new List<object>();

            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    var displayName = item.TryGetProperty("display_name", out var dn) ? dn.GetString() : null;
                    var latStr = item.TryGetProperty("lat", out var l1) ? l1.GetString() : null;
                    var lonStr = item.TryGetProperty("lon", out var l2) ? l2.GetString() : null;

                    if (!string.IsNullOrWhiteSpace(displayName) &&
                        double.TryParse(latStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) &&
                        double.TryParse(lonStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
                    {
                        results.Add(new { displayName, latitude = lat, longitude = lon });
                    }
                }
            }

            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(503, new { message = "No se pudo consultar el servicio de direcciones en este momento. Puedes seleccionar la ubicación directamente en el mapa." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("reverse-geocode")]
    public async Task<IActionResult> ReverseGeocode([FromQuery] double lat, [FromQuery] double lon)
    {
        if (lat < -90 || lat > 90 || lon < -180 || lon > 180)
            return BadRequest(new { message = "Coordenadas fuera de rango válido." });

        var latStr = lat.ToString("F6", CultureInfo.InvariantCulture);
        var lonStr = lon.ToString("F6", CultureInfo.InvariantCulture);
        var cacheKey = $"rev:{latStr},{lonStr}";

        try
        {
            if (!_geoCache.TryGetValue(cacheKey, out var rawJson))
            {
                var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latStr}&lon={lonStr}";
                rawJson = await FetchFromNominatimAsync(url);
                _geoCache[cacheKey] = rawJson;
            }

            using var doc = JsonDocument.Parse(rawJson);
            var root = doc.RootElement;
            var displayName = root.TryGetProperty("display_name", out var dn) ? dn.GetString() : null;

            if (string.IsNullOrWhiteSpace(displayName))
            {
                return Ok(new
                {
                    verified = false,
                    displayName = string.Empty,
                    latitude = lat,
                    longitude = lon
                });
            }

            return Ok(new
            {
                verified = true,
                displayName,
                latitude = lat,
                longitude = lon
            });
        }
        catch (Exception)
        {
            return Ok(new
            {
                verified = false,
                displayName = string.Empty,
                latitude = lat,
                longitude = lon
            });
        }
    }

    private static async Task<string> FetchFromNominatimAsync(string url)
    {
        await _rateLimitSemaphore.WaitAsync();
        try
        {
            var elapsed = DateTime.UtcNow - _lastNominatimRequestUtc;
            if (elapsed < TimeSpan.FromMilliseconds(1050))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1050) - elapsed);
            }

            using var response = await _geoHttpClient.GetAsync(url);
            _lastNominatimRequestUtc = DateTime.UtcNow;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        finally
        {
            _rateLimitSemaphore.Release();
        }
    }
}
