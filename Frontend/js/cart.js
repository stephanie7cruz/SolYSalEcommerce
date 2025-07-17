

document.addEventListener('DOMContentLoaded', () => {
    console.log("cart.js script started.");

    const cartItemsContainer = document.getElementById('cartItemsContainer');
    const emptyCartMessage = document.getElementById('emptyCartMessage');
    const cartSubtotalSpan = document.getElementById('cartSubtotal');
    const cartTotalSpan = document.getElementById('cartTotal');
    const continueShoppingBtn = document.getElementById('continueShoppingBtn');
    const continueToCheckoutBtn = document.getElementById('continueToCheckoutBtn');
    const cartItemCountSpan = document.getElementById('cartItemCount'); // Para el contador en el navbar

    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    // --- Funciones de Utilidad ---

    // Función para formatear precios a moneda colombiana (COP)
    const formatPrice = (price) => {
        return `$${parseFloat(price).toLocaleString('es-CO', { minimumFractionDigits: 0, maximumFractionDigits: 0 })}`;
    };

    // Función para actualizar el contador de ítems en el navbar
    // Esta función se hace global para que main.js pueda llamarla directamente al cargar.
    const updateCartItemCount = () => {
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
        if (cartItemCountSpan) {
            cartItemCountSpan.textContent = totalItems;
        }
    };
    // Hacemos la función global para que main.js pueda inicializar el contador
    window.updateCartItemCountDisplay = updateCartItemCount;


    // Función para guardar el carrito en localStorage
    const saveCart = () => {
        localStorage.setItem('cart', JSON.stringify(cart));
        updateCartUI(); // Actualizar la UI cada vez que se guarda el carrito
        updateCartItemCount(); // Actualizar el contador del navbar después de cada cambio en el carrito
    };

    // --- Funciones de Renderizado ---

    // Función para crear un elemento de producto en el carrito
    const createCartItemElement = (item) => {
        const itemDiv = document.createElement('div');
        itemDiv.className = 'cart-item row align-items-center';
        itemDiv.dataset.id = item.id; // Usar el ID del producto/variante

        itemDiv.innerHTML = `
            <div class="col-12 col-md-6 d-flex align-items-center mb-3 mb-md-0">
                <div class="cart-item-image">
                    <img src="${item.imageUrl || '/images/vestido_general.webp'}" alt="${item.name}">
                </div>
                <div class="cart-item-details">
                    <h5>${item.name}</h5>
                    <p>Talla: ${item.size || 'N/A'} - Color: ${item.color || 'N/A'}</p>
                </div>
            </div>
            <div class="col-6 col-md-2 mb-3 mb-md-0">
                <div class="quantity-control">
                    <button class="btn-minus" data-id="${item.id}">-</button>
                    <input type="number" class="form-control text-center cart-item-qty" value="${item.quantity}" min="1" data-id="${item.id}">
                    <button class="btn-plus" data-id="${item.id}">+</button>
                </div>
            </div>
            <div class="col-6 col-md-2 text-end mb-3 mb-md-0">
                <span class="cart-item-price">${formatPrice(item.price * item.quantity)}</span>
            </div>
            <div class="col-12 col-md-2 text-end">
                <button class="btn-remove" data-id="${item.id}"><i class="fas fa-trash-alt"></i></button>
            </div>
        `;
        return itemDiv;
    };

    // Función principal para actualizar la interfaz de usuario del carrito
    const updateCartUI = () => {
        cartItemsContainer.innerHTML = ''; // Limpiar el contenedor
        let subtotal = 0;

        if (cart.length === 0) {
            emptyCartMessage.style.display = 'block';
            cartItemsContainer.appendChild(emptyCartMessage);
        } else {
            emptyCartMessage.style.display = 'none';
            cart.forEach(item => {
                cartItemsContainer.appendChild(createCartItemElement(item));
                subtotal += item.price * item.quantity;
            });
        }

        cartSubtotalSpan.textContent = formatPrice(subtotal);
        // Por ahora, el total es igual al subtotal (sin envío ni descuentos)
        cartTotalSpan.textContent = formatPrice(subtotal);

    };

    // --- Funciones de Lógica del Carrito ---

    // Función para añadir un producto al carrito
    // Esta función se hace global para que main.js pueda llamarla.
    window.addProductToCart = (product) => {
        // Asegúrate de que el 'product' tenga un ID único (ej. ID de variante)
        // y las propiedades necesarias: id, name, price, imageUrl, size, color
        const existingItemIndex = cart.findIndex(item => item.id === product.id);

        if (existingItemIndex > -1) {
            cart[existingItemIndex].quantity += 1;
        } else {
            cart.push({ ...product, quantity: 1 });
        }
        saveCart(); // Guarda y actualiza la UI y el contador
        console.log("Producto añadido al carrito:", product.name, "Nuevo carrito:", cart);
    };

    // Función para actualizar la cantidad de un producto
    const updateQuantity = (id, newQuantity) => {
        const itemIndex = cart.findIndex(item => item.id === id);
        if (itemIndex > -1) {
            newQuantity = parseInt(newQuantity);
            if (newQuantity > 0) {
                cart[itemIndex].quantity = newQuantity;
            } else {
                // Si la cantidad es 0 o menos, eliminar el artículo
                cart.splice(itemIndex, 1);
            }
            saveCart(); // Guarda y actualiza la UI y el contador
        }
    };

    // Función para eliminar un producto del carrito
    const removeItem = (id) => {
        cart = cart.filter(item => item.id !== id);
        saveCart(); // Guarda y actualiza la UI y el contador
    };

    // --- Event Listeners ---

    // Manejar clics en los botones de cantidad y eliminar
    cartItemsContainer.addEventListener('click', (event) => {
        const target = event.target;
        const itemId = target.dataset.id || target.closest('button')?.dataset.id; // Manejar clic en icono o botón

        if (!itemId) return; // Si no hay ID, salir

        if (target.classList.contains('btn-minus')) {
            const input = target.nextElementSibling;
            const currentQty = parseInt(input.value);
            if (currentQty > 1) {
                input.value = currentQty - 1;
                updateQuantity(itemId, input.value);
            } else {
                // Si la cantidad es 1 y se presiona '-', eliminar el ítem
                removeItem(itemId);
            }
        } else if (target.classList.contains('btn-plus')) {
            const input = target.previousElementSibling;
            input.value = parseInt(input.value) + 1;
            updateQuantity(itemId, input.value);
        } else if (target.classList.contains('btn-remove') || target.closest('.btn-remove')) {
            // Manejar clic en el icono de la papelera o el botón que lo contiene
            removeItem(itemId);
        }
    });

    // Manejar cambios manuales en el input de cantidad
    cartItemsContainer.addEventListener('change', (event) => {
        const target = event.target;
        if (target.classList.contains('cart-item-qty')) {
            const itemId = target.dataset.id;
            let newQuantity = parseInt(target.value);
            if (isNaN(newQuantity) || newQuantity < 1) {
                newQuantity = 1; // Asegurar que la cantidad sea al menos 1
                target.value = 1;
            }
            updateQuantity(itemId, newQuantity);
        }
    });

    // Botones de navegación
    if (continueShoppingBtn) {
        continueShoppingBtn.addEventListener('click', () => {
            window.location.href = 'home.html'; // Redirigir a la página principal
        });
    }

    if (continueToCheckoutBtn) {
        continueToCheckoutBtn.addEventListener('click', () => {
            // Redirigir al portal de pagos de Wompi
            // La URL de Wompi es https://checkout.wompi.co/l/VPOS_8ZXxur
            window.location.href = 'https://checkout.wompi.co/l/VPOS_8ZXxur';
        });
    }

    // Inicializar la UI del carrito al cargar la página
    updateCartUI();
    // updateCartItemCount(); // Ya se llama desde updateCartUI, que a su vez es llamada por saveCart
});

// Las funciones addProductToCart y updateCartItemCountDisplay ya se hacen globales dentro del DOMContentLoaded
// para asegurar que los elementos del DOM estén disponibles al inicializarlas.
