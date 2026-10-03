# Destino de Ensueños

**Plataforma web turística** desarrollada como proyecto final universitario para la asignatura Herramientas de Programación III.

---

## Descripción

Destino de Ensueños es una plataforma web que permite a los usuarios descubrir lugares, negocios, restaurantes, emprendimientos, miradores, eventos y descuentos de una región o ciudad. La plataforma cuenta con un directorio turístico/comercial, sistema de valoraciones y reseñas, mapa interactivo, agenda de eventos y descuentos, y un panel de administración.

---

## Tecnologías utilizadas

| Capa | Tecnología |
|------|------------|
| Backend | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Base de datos | SQL Server (LocalDB o instancia local) |
| Autenticación | ASP.NET Core Identity + JWT Bearer |
| Frontend | HTML5, CSS3, JavaScript (Vanilla) |
| Mapas | Leaflet 1.9.4 + OpenStreetMap |
| Documentación API | Swagger / OpenAPI |

---

## Requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server](https://www.microsoft.com/sql-server) (Express, Developer o LocalDB)
- [Node.js](https://nodejs.org/) (para el servidor HTTP del frontend)
- `dotnet-ef` 8.x instalado como herramienta global

Para instalar `dotnet-ef`:
```bash
dotnet tool install --global dotnet-ef --version 8.*
```

---

## Instalación

1. Clonar o descomprimir el repositorio en una carpeta local.
2. Restaurar dependencias del backend:
   ```bash
   cd DestinoEnsueno.API
   dotnet restore
   ```

---

## Configuración

Editar `DestinoEnsueno.API/appsettings.json` con los valores del entorno:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<SERVIDOR>;Database=DestinoEnsuenosDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "<CLAVE_SECRETA_MINIMO_32_CARACTERES>",
    "Issuer": "DestinoEnsuenosAPI",
    "Audience": "DestinoEnsuenosApp"
  }
}
```

Reemplazar `<SERVIDOR>` con el nombre de la instancia de SQL Server (por ejemplo `localhost` o `localhost\SQLEXPRESS`) y `<CLAVE_SECRETA_MINIMO_32_CARACTERES>` con una clave segura.

---

## Base de datos

La migración inicial ya está incluida en el proyecto. Al iniciar la aplicación por primera vez, Entity Framework Core aplica automáticamente las migraciones y ejecuta el seed de datos (roles, usuarios de prueba, categorías y negocios de ejemplo).

Para aplicar migraciones manualmente:
```bash
cd DestinoEnsueno.API
dotnet ef database update
```

---

## Ejecución

### Opción A — Script automático (recomendado)

Ejecutar `iniciar-proyecto.bat` desde la raíz del proyecto. Abre el backend y el frontend en ventanas separadas y lanza el navegador automáticamente.

### Opción B — Manual

**Backend (API):**
```bash
cd DestinoEnsueno.API
dotnet run
```
La API queda disponible en `http://localhost:5058`.  
La documentación Swagger en `http://localhost:5058/swagger`.

**Frontend:**
```bash
node .gemini-serve.js
```
El frontend queda disponible en `http://localhost:5500`.

> El backend debe estar activo antes de usar el frontend.

---

## Estructura del proyecto

```
Destino de ensueño - Proyecto final/
├── DestinoEnsueno.API/          # Backend ASP.NET Core
│   ├── Controllers/             # Controladores de la API REST
│   ├── Data/                    # DbContext y seed de datos
│   ├── DTOs/                    # Objetos de transferencia de datos
│   ├── Migrations/              # Migraciones de Entity Framework Core
│   ├── Models/                  # Entidades del dominio
│   ├── Services/                # Lógica de negocio (interfaces e implementaciones)
│   ├── appsettings.json         # Configuración de la aplicación
│   └── Program.cs               # Punto de entrada y configuración del pipeline
├── Frontend/                    # Frontend estático
│   ├── css/style.css            # Hoja de estilos principal
│   ├── js/
│   │   ├── api.js               # Cliente HTTP centralizado
│   │   ├── app.js               # Navbar dinámica y utilidades globales
│   │   └── auth.js              # Manejo de inicio de sesión
│   ├── index.html               # Página de inicio
│   ├── negocios.html            # Directorio de negocios
│   ├── negocio.html             # Detalle de un negocio
│   ├── agenda.html              # Eventos y descuentos
│   ├── mapa.html                # Mapa interactivo
│   ├── panel-admin.html         # Panel de administración
│   ├── login.html               # Inicio de sesión
│   └── registro.html            # Registro de usuario
├── .gemini-serve.js             # Servidor HTTP local del frontend
├── iniciar-proyecto.bat         # Script de inicio completo
├── iniciar-api.bat              # Script de inicio solo del backend
└── iniciar-frontend.bat         # Script de inicio solo del frontend
```

---

## Funcionalidades principales

- **Directorio turístico**: listado de negocios, restaurantes, emprendimientos y miradores con búsqueda por texto y filtro por categoría.
- **Detalle de negocio**: información completa, mapa integrado con ubicación exacta, valoración promedio y lista de reseñas.
- **Mapa interactivo**: visualización geográfica de todos los negocios publicados con popups informativos.
- **Agenda**: sección de eventos próximos y descuentos activos con pestañas independientes.
- **Sistema de valoraciones**: escala de 1 a 5 estrellas. Un usuario puede dejar una sola reseña por negocio (creación o actualización).
- **Autenticación**: registro e inicio de sesión con JWT. Redirección automática según rol.
- **Panel de administración**: creación de negocios, eventos y descuentos. Acceso restringido al rol Admin.

---

## API — Endpoints principales

| Método | Ruta | Acceso | Descripción |
|--------|------|--------|-------------|
| POST | `/api/Auth/register` | Público | Registro de usuario |
| POST | `/api/Auth/login` | Público | Inicio de sesión |
| POST | `/api/Auth/register-admin` | Admin | Crear cuenta de administrador |
| GET | `/api/Business` | Público | Listado de negocios publicados |
| GET | `/api/Business/{id}` | Público | Detalle de un negocio |
| POST | `/api/Business` | Admin | Crear negocio |
| GET | `/api/Category` | Público | Listado de categorías |
| GET | `/api/Event/upcoming` | Público | Próximos eventos publicados |
| POST | `/api/Event` | Admin | Crear evento |
| GET | `/api/Discount/active` | Público | Descuentos activos publicados |
| POST | `/api/Discount` | Admin | Crear descuento |
| GET | `/api/Review/{businessId}` | Público | Reseñas de un negocio |
| POST | `/api/Review` | Usuario autenticado | Crear o actualizar reseña |

---

## Autenticación y roles

El sistema utiliza JWT Bearer con dos roles:

| Rol | Acceso |
|-----|--------|
| **Admin** | Panel de administración, creación de contenido, gestión de la plataforma |
| **User** | Exploración del contenido, escritura de reseñas y valoraciones |

El registro público asigna siempre el rol `User`. La creación de cuentas `Admin` requiere autenticación previa con un administrador existente.

---

## Mapas

El mapa interactivo utiliza **Leaflet 1.9.4** con tiles de **OpenStreetMap** (`https://tile.openstreetmap.org/{z}/{x}/{y}.png`), de uso libre bajo la [Tile Usage Policy de OpenStreetMap](https://operations.osmfoundation.org/policies/tiles/). No se requiere API key.

---

## Credenciales de desarrollo

Las siguientes credenciales son generadas automáticamente al iniciar la aplicación por primera vez y están destinadas exclusivamente a entornos de prueba:

| Rol | Email | Contraseña |
|-----|-------|------------|
| Admin | `admin@destino.com` | `Admin123!` |
| Usuario | `user@destino.com` | `User123!` |

**No utilizar estas credenciales en producción.**

---

## Estado del proyecto

Versión de desarrollo completa con todas las funcionalidades implementadas. Proyecto académico.
