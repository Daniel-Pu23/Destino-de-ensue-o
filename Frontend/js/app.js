document.addEventListener('DOMContentLoaded', () => {
    renderNavbar();
});

function renderNavbar() {
    const navLinks = document.getElementById('nav-links');
    if (!navLinks) return;

    const token = localStorage.getItem('token');
    const role = localStorage.getItem('role');
    const name = localStorage.getItem('firstName');

    let html = `
        <li><a href="index.html">Inicio</a></li>
        <li><a href="negocios.html">Directorio</a></li>
        <li><a href="agenda.html">Agenda</a></li>
        <li><a href="mapa.html">Mapa</a></li>
    `;

    if (token) {
        if (role === 'Admin') {
            html += `<li><a href="panel-admin.html" style="color:#e74c3c; font-weight:bold;">⚙ Panel Admin</a></li>`;
        }
        html += `
            <li><span class="nav-greeting">Hola, ${name}</span></li>
            <li><a href="#" onclick="logout(event)" class="btn-logout">Cerrar Sesión</a></li>
        `;
    } else {
        html += `
            <li><a href="login.html" class="btn btn-primary" style="color:white; padding: 8px 16px;">Ingresar</a></li>
            <li><a href="registro.html">Registrarse</a></li>
        `;
    }

    navLinks.innerHTML = html;
}

function logout(e) {
    e.preventDefault();
    localStorage.clear();
    window.location.href = 'index.html';
}

// Muestra un mensaje de estado en un elemento
function showStatus(elementId, message, isError = false) {
    const el = document.getElementById(elementId);
    if (!el) return;
    el.textContent = message;
    el.style.color = isError ? 'red' : 'green';
}

// Renderiza estrellas visuales dado un rating promedio
function renderStars(avg) {
    const full = Math.round(avg);
    let stars = '';
    for (let i = 1; i <= 5; i++) {
        stars += i <= full ? '★' : '☆';
    }
    return `<span class="stars" style="color:#f39c12;">${stars}</span>`;
}
