// Servidor HTTP local para la carpeta Frontend — Destino de Ensueños
// Ejecutar con: node .gemini-serve.js
// Acceder en:   http://localhost:5500

const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 5500;
const ROOT = path.join(__dirname, 'Frontend');
const MIME = {
    'html': 'text/html; charset=utf-8',
    'css':  'text/css',
    'js':   'application/javascript',
    'png':  'image/png',
    'jpg':  'image/jpeg',
    'ico':  'image/x-icon',
    'json': 'application/json'
};

http.createServer((req, res) => {
    // Separar el query string antes de resolver la ruta del archivo
    const urlPath = (req.url === '/' ? '/index.html' : req.url).split('?')[0];
    const filePath = path.join(ROOT, urlPath);

    fs.readFile(filePath, (err, data) => {
        if (err) {
            res.writeHead(404, { 'Content-Type': 'text/plain' });
            res.end('404 – No encontrado: ' + urlPath);
        } else {
            const ext = path.extname(filePath).slice(1).toLowerCase();
            res.writeHead(200, { 'Content-Type': MIME[ext] || 'application/octet-stream' });
            res.end(data);
        }
    });
}).listen(PORT, '127.0.0.1', () => {
    console.log('==========================================================');
    console.log('  Frontend disponible en: http://localhost:' + PORT);
    console.log('  Presiona Ctrl+C para detener el servidor.');
    console.log('==========================================================');
});
