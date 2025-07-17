# README: Aplicación Sol y Sal Ecommerce

Este documento describe la arquitectura, las tecnologías utilizadas, las características de seguridad, las APIs disponibles y las instrucciones para ejecutar la aplicación de comercio electrónico "Sol y Sal".

## 1. Arquitectura de la Aplicación

La aplicación sigue una arquitectura de **frontend y backend separados (API RESTful)**, lo que permite el desarrollo y despliegue independiente de ambas partes.

* **Frontend (Cliente Web):** Desarrollado en HTML, CSS y JavaScript, se ejecuta en el navegador del usuario. Es responsable de la interfaz de usuario, la lógica de presentación y la interacción con el usuario. Se comunica con el backend a través de llamadas HTTP a la API.

* **Backend (API RESTful):** Desarrollado con ASP.NET Core Web API en C#, es el cerebro de la aplicación. Maneja la lógica de negocio, la persistencia de datos (aunque actualmente el frontend usa datos hardcodeados para productos), la autenticación de usuarios y expone los endpoints de la API para que el frontend interactúe con ellos.

## 2. Tecnologías Utilizadas

### 2.1. Frontend

* **HTML5:** Estructura de la página web.

* **CSS3:** Estilos y diseño de la interfaz de usuario.

    * **Bootstrap 5:** Framework CSS para un diseño responsivo y componentes de UI predefinidos.

    * **Custom CSS (`style.css`, `cart-styles.css`, `profile-styles.css`):** Estilos personalizados para la marca y el diseño específico de Sol y Sal.

* **JavaScript (ES6+):** Lógica interactiva del lado del cliente.

    * **`main.js`:** Gestión de la visualización de productos, filtrado por categoría y **búsqueda en tiempo real**. Actualmente, carga los datos de productos desde CSVs hardcodeados.

    * **`cart.js`:** Gestión del carrito de compras, utilizando `localStorage` para la persistencia de los ítems del carrito en el lado del cliente. No realiza llamadas a la API para el carrito.

    * **`auth.js`:** Manejo de la lógica de autenticación (registro e inicio de sesión). **Actualmente, esta versión simula la autenticación sin usar llamadas AJAX a la API del backend.**

    * **`profile.js`:** Gestiona la visualización y edición de la información del perfil del usuario y la carga de pedidos (aunque la carga de pedidos está configurada para usar `api.js`, si `auth.js` no usa AJAX, esta parte no funcionará como se espera con el backend).

    * **`api.js`:** (Este archivo existe en el historial, pero no está siendo usado por `auth.js` o `cart.js` en las últimas versiones que solicitaste. Su propósito es centralizar las llamadas AJAX a la API del backend, incluyendo el manejo de tokens JWT y errores).

### 2.2. Backend

* **ASP.NET Core Web API (C#):** Framework para construir la API RESTful.

* **Autenticación JWT (JSON Web Tokens):** Para asegurar los endpoints de la API y gestionar las sesiones de usuario.

* **Base de Datos (Intención):** Aunque no se ha implementado explícitamente en el frontend con AJAX en las últimas versiones, el backend está diseñado para interactuar con una base de datos (probablemente SQL Server con Entity Framework Core) para la persistencia de usuarios, productos y órdenes.

## 3. Estructura del Proyecto Backend

El proyecto de backend de ASP.NET Core sigue una estructura organizada para separar las responsabilidades y facilitar el mantenimiento. Aunque la implementación exacta puede variar, típicamente incluye las siguientes capas y componentes:

* **Controladores (`Controllers/`):** Contienen la lógica para manejar las solicitudes HTTP entrantes. Cada controlador es responsable de un recurso específico (ej., `AuthController`, `ProductsController`, `OrdersController`) y define los endpoints de la API.

* **Modelos de Datos (Entities/Models):** Representan la estructura de los datos que se almacenan en la base de datos (ej., `User`, `Product`, `Order`).

* **DTOs (Data Transfer Objects):** Objetos utilizados para transferir datos entre las capas de la aplicación, especialmente entre los controladores y los servicios, o para definir el formato de los datos de entrada/salida de la API (ej., `RegisterRequestDto`, `LoginRequestDto`, `ProductDto`). Esto ayuda a desacoplar los modelos de dominio de la exposición de la API y a controlar qué datos se envían.

* **Interfaces y Repositorios (`Interfaces/`, `Repositories/`):** Definen contratos para la interacción con la base de datos o fuentes de datos externas. Los repositorios implementan estas interfaces para manejar las operaciones CRUD (Crear, Leer, Actualizar, Eliminar) de los modelos de datos.

* **Servicios (`Services/`):** Contienen la lógica de negocio principal de la aplicación. Interactúan con los repositorios para realizar operaciones complejas y coordinar las acciones entre diferentes modelos de datos.

* **Configuración (`appsettings.json`, `Startup.cs`):** Archivos para configurar la aplicación, como cadenas de conexión a la base de datos, configuraciones de JWT, CORS, y el registro de dependencias.

## 4. Seguridad Aplicada

* **Autenticación Basada en Tokens (JWT):** El backend utiliza JWT para verificar la identidad de los usuarios. Una vez que un usuario inicia sesión, recibe un token que debe incluir en las solicitudes posteriores a los endpoints protegidos.

* **Validación de Entrada:** El backend implementa validaciones de datos (ej., campos requeridos como `FirstName`, `LastName`, `Email`, `Password` para el registro) para prevenir datos inválidos o maliciosos.

* **HTTPS (Implícito en Desarrollo):** El uso de `https://localhost:7191` implica que la comunicación entre el frontend y el backend se realiza a través de HTTPS, proporcionando cifrado de datos en tránsito.

* **CORS (Cross-Origin Resource Sharing):** El backend debe estar configurado para permitir solicitudes desde el dominio donde se sirve el frontend (ej., `localhost` durante el desarrollo) para evitar problemas de seguridad del navegador.

## 5. APIs Disponibles (Backend)

El backend expone los siguientes endpoints principales:

* **`POST /api/Auth/register`:** Registra un nuevo usuario. Espera un cuerpo JSON con `email`, `password`, `firstName`, `lastName`.

* **`POST /api/Auth/login`:** Autentica a un usuario existente. Espera un cuerpo JSON con `email` y `password`. Devuelve un token JWT si las credenciales son válidas.

* **`GET /api/Products`:** Obtiene una lista de productos.

* **`GET /api/Orders`:** (Protegido con JWT) Obtiene los pedidos del usuario autenticado.

* **`POST /api/Orders`:** (Protegido con JWT) Crea un nuevo pedido.

## 6. Cómo Correr la Aplicación

Para ejecutar la aplicación, necesitarás tener tanto el backend como el frontend funcionando.

### 6.1. Requisitos Previos

* **.NET SDK:** Necesario para compilar y ejecutar el proyecto de backend de C#.

* **Navegador Web:** Cualquier navegador moderno (Chrome, Firefox, Edge, Safari).

* **Node.js y npm (para `http-server`):** Necesarios para instalar y ejecutar `http-server`.

### 6.2. Ejecutar el Backend (API de C#)

1.  **Navega al directorio del proyecto de backend:** Abre tu terminal o línea de comandos y ve a la carpeta raíz de tu proyecto ASP.NET Core (donde se encuentra el archivo `.csproj`).

2.  **Restaura las dependencias (si es la primera vez):**

    ```bash
    dotnet restore
    ```

3.  **Compila el proyecto:**

    ```bash
    dotnet build
    ```

4.  **Ejecuta el proyecto:**

    ```bash
    dotnet run
    ```

    Esto iniciará el servidor de desarrollo, generalmente en `https://localhost:7191` (o un puerto similar). Verás mensajes en la consola indicando que el servidor está escuchando. **Asegúrate de que la URL `https://localhost:7191/api` sea accesible.**

### 6.3. Ejecutar el Frontend (Cliente Web)

**¡Importante! Asegúrate de que el backend esté corriendo antes de iniciar el frontend.**

1.  **Instala `http-server` (si no lo tienes):** Abre tu terminal o línea de comandos y ejecuta:

    ```bash
    npm install -g http-server
    ```

2.  **Navega a la carpeta del frontend:** En tu terminal o línea de comandos, ve a la carpeta donde se encuentran tus archivos `home.html`, `css`, `js`, etc. (la raíz de tu frontend).

3.  **Ejecuta el servidor:**

    ```bash
    http-server
    ```

    Esto iniciará un servidor web local, generalmente en el puerto `8080`.

4.  **Abre la aplicación en tu navegador:** Una vez que el servidor esté corriendo, abre tu navegador web y navega a la siguiente URL:

    ```
    http://localhost:8080/home.html
    ```

5.  **Verifica la consola del navegador:** Abre las herramientas de desarrollador de tu navegador (F12 o Ctrl+Shift+I) y ve a la pestaña "Console" para ver los mensajes de depuración de JavaScript.

6.  **Prueba la búsqueda:** Utiliza el campo de búsqueda en la parte superior para buscar productos por nombre (ej., "Flora", "Maia") o por categoría (ej., "Bikini", "Enterizo", "Salidas").

7.  **Prueba las categorías:** Haz clic en los enlaces de categoría en la barra de navegación (Bikinis, Enterizos, etc.) para filtrar los productos.

8.  **Prueba el carrito:** Añade productos al carrito. Los ítems se guardarán en tu `localStorage`.

9.  **Prueba la autenticación (simulada):**

    * Ve a `auth.html`.

    * Puedes intentar "iniciar sesión" con `test@example.com` y `password123`.

    * El registro solo mostrará un mensaje simulado de éxito/fallo, ya que no se comunica con el backend en esta versión.

---

**Imagenes:**
<img width="1917" height="1076" alt="image" src="https://github.com/user-attachments/assets/0d1e5765-909f-4c6b-af9c-94121583cbfa" />
<img width="1916" height="1015" alt="image" src="https://github.com/user-attachments/assets/d0e21303-a132-45a1-a8c9-895edd0971db" />
<img width="1904" height="1014" alt="image" src="https://github.com/user-attachments/assets/a78c8039-f0ea-4225-b0a3-2864cacff81c" />
<img width="1911" height="1015" alt="image" src="https://github.com/user-attachments/assets/bef95c6b-074c-49d3-8efb-6342ba93be8b" />
<img width="1913" height="1015" alt="image" src="https://github.com/user-attachments/assets/2e869233-af8b-44db-b650-0bee59c96f84" />
<img width="1915" height="1019" alt="image" src="https://github.com/user-attachments/assets/5115a6fc-5a31-4818-81f7-27d2d71b9a02" />
<img width="1911" height="1012" alt="image" src="https://github.com/user-attachments/assets/6be2212a-0fe6-4358-ac79-7aa53424de53" />
<img width="1915" height="1015" alt="image" src="https://github.com/user-attachments/assets/98be64b4-e8e6-48f1-90a8-8ddf4652d700" />
<img width="1919" height="1016" alt="image" src="https://github.com/user-attachments/assets/f5bf0595-4c9f-408a-b7d3-cfe29e5cd26b" />
<img width="1913" height="1013" alt="image" src="https://github.com/user-attachments/assets/7ece1c1b-a1fd-44aa-b83b-149b9c6553d1" />


