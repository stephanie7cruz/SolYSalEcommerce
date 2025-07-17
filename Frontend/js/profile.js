import { apiCall } from './api.js'; // Necesitas api.js para hacer llamadas al backend

console.log("profile.js script started.");

document.addEventListener('DOMContentLoaded', async () => {
    console.log("DOM Content Loaded in profile.js.");

    const jwtToken = localStorage.getItem('jwtToken');

    // Redirigir si no hay token
    if (!jwtToken) {
        console.log("No JWT token found, redirecting to auth.html");
        window.location.href = 'auth.html';
        return;
    }

    // Mostrar navbar colapsado en móviles
    const navbarCollapse = document.getElementById('navbarNav');
    if (navbarCollapse && window.innerWidth < 992) {
        navbarCollapse.classList.add('show');
    }

    // --- Actualizar la barra superior ---
    const profileLink = document.getElementById('profileLink');
    const profileText = document.getElementById('profileText');
    const logoutButton = document.getElementById('logoutButton');

    if (jwtToken) {
        console.log("Usuario logueado (JWT token encontrado). Actualizando navbar para estado logueado.");
        if (profileText) profileText.textContent = 'Tu Cuenta';
        if (profileLink) profileLink.href = 'profile.html';
        if (logoutButton) {
            logoutButton.style.setProperty('display', 'block', 'important');
            console.log("Botón de Cerrar Sesión visible (display: block !important).");
        } else {
            console.warn("Elemento con ID 'logoutButton' no encontrado.");
        }
    }

    // --- Barra lateral y navegación de secciones ---
    const sidebarLinks = document.querySelectorAll('.sidebar .nav-link');
    const profileSections = document.querySelectorAll('.profile-section-content');

    const showSection = (sectionId) => {
        profileSections.forEach(section => section.style.display = 'none');
        const activeSection = document.getElementById(sectionId);
        if (activeSection) activeSection.style.display = 'block';
    };

    sidebarLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            sidebarLinks.forEach(item => item.classList.remove('active'));
            link.classList.add('active');
            showSection(link.getAttribute('data-target'));
        });
    });

    showSection('profileDetailsSection');
    document.querySelector('.sidebar .nav-link[data-target="profileDetailsSection"]').classList.add('active');

    // --- Mostrar datos del perfil desde localStorage ---
    const profileEmailSpan = document.getElementById('profileEmail');
    const profileEmailInput = document.getElementById('profileEmailInput');
    const profileEmailEditIcon = document.getElementById('profileEmailEditIcon');
    const profileNameSpan = document.getElementById('profileName');
    const ordersList = document.getElementById('ordersList');
    const welcomeUserName = document.getElementById('welcomeUserName');

    const storedEmail = localStorage.getItem('userEmail');
    const storedFirstName = localStorage.getItem('userFirstName');
    const storedLastName = localStorage.getItem('userLastName');

    if (profileEmailSpan) profileEmailSpan.textContent = storedEmail || 'N/A';
    if (profileEmailInput) profileEmailInput.value = storedEmail || 'N/A';
    if (profileNameSpan) profileNameSpan.textContent = `${storedFirstName || ''} ${storedLastName || ''}`.trim() || 'N/A';

    if (welcomeUserName && storedFirstName) {
        welcomeUserName.textContent = `, ${storedFirstName}`;
    } else if (welcomeUserName && storedEmail) {
        welcomeUserName.textContent = `, ${storedEmail.split('@')[0]}`;
    }

    let isEditingEmail = false;

    if (profileEmailEditIcon && profileEmailSpan && profileEmailInput) {
        profileEmailEditIcon.addEventListener('click', () => {
            if (isEditingEmail) {
                profileEmailInput.style.display = 'none';
                profileEmailSpan.style.display = 'inline';
                profileEmailEditIcon.classList.remove('fa-save');
                profileEmailEditIcon.classList.add('fa-edit');
                localStorage.setItem('userEmail', profileEmailInput.value);
                profileEmailSpan.textContent = profileEmailInput.value;
                console.log('Email actualizado localmente:', profileEmailInput.value);
            } else {
                profileEmailSpan.style.display = 'none';
                profileEmailInput.style.display = 'inline';
                profileEmailInput.value = profileEmailSpan.textContent;
                profileEmailInput.focus();
                profileEmailEditIcon.classList.remove('fa-edit');
                profileEmailEditIcon.classList.add('fa-save');
            }
            isEditingEmail = !isEditingEmail;
        });
    }

    // --- Cargar pedidos ---
    try {
        const orders = await apiCall('/Orders', 'GET', null, jwtToken);
        if (ordersList) {
            if (orders && orders.length > 0) {
                ordersList.innerHTML = '';
                orders.forEach(order => {
                    const li = document.createElement('li');
                    li.className = 'list-group-item';
                    li.textContent = `Pedido #${order.id} - Fecha: ${new Date(order.orderDate).toLocaleDateString()} - Total: $${order.total.toFixed(2)}`;
                    ordersList.appendChild(li);
                });
            } else {
                ordersList.innerHTML = '<li class="list-group-item">No tienes pedidos aún.</li>';
            }
        }
    } catch (error) {
        console.error('Error al cargar pedidos:', error);
        if (ordersList) ordersList.innerHTML = '<li class="list-group-item text-danger">Error al cargar los pedidos.</li>';
    }

    // --- Cerrar sesión ---
    if (logoutButton) {
        logoutButton.addEventListener('click', () => {
            console.log("Cerrando sesión...");
            localStorage.removeItem('jwtToken');
            localStorage.removeItem('userName');
            localStorage.removeItem('userEmail');
            localStorage.removeItem('userFirstName');
            localStorage.removeItem('userLastName');
            window.location.href = 'auth.html';
        });
    }
});
