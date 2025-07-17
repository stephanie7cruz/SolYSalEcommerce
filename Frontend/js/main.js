

// Contenido del CSV DBO.PRODUCTS
const rawProductsCsvContent = `Id,Name,Description,BasePrice,IsActive
06F2B21C-0832-4FF0-A3CD-0D9603C0C91E,Bikinitest2,Bikini,60000.00,1
58FB4E14-38CB-4023-829B-1CD86CAAF303,Flora,Bikini,59900.00,1
C7821BE9-0385-4818-A5C1-1DEFD7A08C3F,Carmen,Enterizo,72500.00,1
CD93405D-5A5B-4170-9DCD-1E78E1C3977C,Juliette,Serena,69900.00,1
991D1B30-3055-4496-997C-2815CD45C6F4,Rosa,Bikini Set,50000.00,1
0C8BB861-D5D3-457A-AF43-2ED3F23DEF2D,Rana,Bikini,59900.00,1
3F3BB5EE-8047-41A6-9571-329771E43427,Salida Dafne,Salidas,25000.00,1
4431B2FF-DAE5-43E1-AD96-3DDEB4495995,Lina,Bikini,59900.00,1
CBDB7982-561B-423C-9A1A-3FBFC42C4B1A,Emilia,Serena,69900.00,1
DDBA88EC-646B-4F18-9E21-4216198C339F,Zoe,Bikini,59900.00,1
2AC8AE15-12E5-46AE-8354-46BB6ADAE710,Allie,Bikini,59900.00,1
2F9C8343-921B-4567-BF92-4BBA8ABC649D,Sobrer Carla,Accesorios,25000.00,1
C9E37258-B92C-4089-99C6-5CCA88F0EE5D,Lorena,Serena,69900.00,1
88D95559-2057-4139-8512-6026D362830C,Mat,Bikini Set,50000.00,1
E97160D6-BF84-400C-B8E3-6DB98E737F87,Renata,Serena,69900.00,1
EA5CF076-62E8-4A83-94CC-70660CF583AF,Ventosa,Bikini,69900.00,1
984F83DB-14FA-4B97-B0DF-749F6630BCC1,Pamela,Bikini,59900.00,1
C9E8A2DB-C7D0-4F7C-91EE-77DB5C09D2D3,Lia,Bikini,59900.00,1
1E075966-8FDF-4415-83BC-7E7C1A712E9A,Lara,Bikini Set,60950.00,1
69FE73BE-431A-4190-A99C-7F944EAB92A9,Salida Anna,Salidas,25000.00,1
034F3F12-1148-4393-9255-8353EA214CF5,Kimono perla,Salidas,60000.00,1
77D68A19-BA27-47E7-A516-860FAB6F3B2B,Salida Lisa,Salidas,25000.00,1
29F3F7EB-402C-47D1-BA17-A5F4901D9498,Pruebita,bikini,50000.00,1
E3340D28-8314-45BB-AB35-ADB9B4B6858B,Eliana,Bikini,59900.00,1
096A91F7-7060-475A-B31B-B7F9593BEFA7,Lisette,Serena,69900.00,1
A456E3D5-23D3-4C01-8C05-CFDC56B6EED1,Celeste,Enterizo,72500.00,1
D75C8E9C-F8CD-4BC6-B95C-D22A2905D841,Kimono arena,Salidas,45500.00,1
103187B4-13D3-49E8-ADD5-D31F1F8C13B7,Kimono andrea,Salidas,55000.00,1
5483147B-FC5E-4D5C-814B-D3C85522D20D,Sublime,Bikini,57500.00,1
B4AB5517-E93F-4B61-AA76-D54610188B2A,Salida Liora,Salidas,30000.00,1
ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Dahian,Bikini,59900.00,1
4C16B4F7-E4E4-4337-84CB-E1C9637A2D7D,Antonia,Bikini,60000.00,1
63AF4702-1CDE-4E7A-97DE-E536E3039997,Salida Dana,Salidas,25000.00,1
6650CDF9-A85B-4BA3-8EC7-E5F949CD2329,Amber,Bikini,59900.00,1
66322DC5-3C43-4F39-BFA4-E967EA8D7E75,Bikinitest,Bikini,60000.00,1
A78AC3E1-91B9-4745-9F24-ED2F38B5747F,Caribeña,Bikini,60000.00,1
9F60E648-A129-4D75-9404-F7671F9E81B1,Bruna,Bikini Set,65000.00,1
2FFD1D7E-08C9-42DC-9A95-FDC003EEECCA,Maia,Bikini,59900.00,1
52E96DB1-0CA1-4B79-8F18-FF3FE2E56B81,Salida Dalia,Salidas,25000.00,1
902C0FE2-04A1-4D46-BA96-FF6A17D7B0D9,bikinitest2,bikini,55000.00,1
`;

// Contenido del CSV ProductVariants
const rawProductVariantsCsvContent = `Id,ProductId,Color,Size,SKU,Stock,ImagenUrl,BasePrice
6DEF4F9B-D4CC-4ED2-8A8C-01B731739312,3F3BB5EE-8047-41A6-9571-329771E43427,Blanco,M,SALIDADAFNE-BLANCO-M,1,/images/vestido_general.webp,25000.00
BBD85B8F-A4E1-461B-8137-02DFB66267CF,A78AC3E1-91B9-4745-9F24-ED2F38B5747F,Rojo,M,CARIBEÑA-ROJO-M,1,/images/vestido_general.webp,60000.00
B6329E91-3947-4CD5-B8C5-0362211A75B3,29F3F7EB-402C-47D1-BA17-A5F4901D9498,negro,M,PRUEBILLA-NEGRO-M,1,/images/vestido_general.webp,50000.00
D3B040A1-B731-4AF5-BCD1-053DBAD7C6F4,E3340D28-8314-45BB-AB35-ADB9B4B6858B,Negro,M,ELIANA-NEGRO-M,1,/images/vestido_general.webp,59900.00
D2F9EAFE-2A95-4CAE-A072-0A00B1DEE7E0,DDBA88EC-646B-4F18-9E21-4216198C339F,Negro,M,ZOE-NEGRO-M,1,/images/vestido_general.webp,59900.00
D70785BA-71C3-4CF3-9F94-0BFBC8DB4876,ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Negro,L,DAHIAN-NEGRO-L,1,/images/vestido_general.webp,59900.00
B449E0EF-D58A-482E-ACFA-0C002112967D,5483147B-FC5E-4D5C-814B-D3C85522D20D,Negro,M,SUBLIME-NEGRO-M,1,/images/vestido_general.webp,57500.00
B57D9284-19C4-43D9-A7D2-106E0AFE35A2,4C16B4F7-E4E4-4337-84CB-E1C9637A2D7D,Blanco,M,ANTONIA-BLANCO-M,1,/images/vestido_general.webp,60000.00
F0A96C41-FB28-49D0-8D63-13A84AFF84BD,4431B2FF-DAE5-43E1-AD96-3DDEB4495995,Rojo,M,LINA-ROJO-M,1,/images/vestido_general.webp,59900.00
83E6F15A-F905-4840-81E0-1597E10A5074,58FB4E14-38CB-4023-829B-1CD86CAAF303,Verde,M,FLORA-VERDE-M,1,/images/vestido_general.webp,59900.00
0583A6EA-F9D8-40A1-89C5-15DF377E5103,096A91F7-7060-475A-B31B-B7F9593BEFA7,Blanco,M,LISETTE-BLANCO-M,1,/images/vestido_general.webp,69900.00
3CBA0A61-B241-4F93-9112-1B5D77AE3050,991D1B30-3055-4496-997C-2815CD45C6F4,Blanco,M,ROSA-BLANCO-M,1,/images/vestido_general.webp,50000.00
6A76BCC7-78D5-47BC-8BD5-227761800CB5,034F3F12-1148-4393-9255-8353EA214CF5,Blanco,M,KIMONOPERLA-BLANCO-M,1,/images/vestido_general.webp,60000.00
C699945C-C3CE-4020-A700-2468E00BFBFB,06F2B21C-0832-4FF0-A3CD-0D9603C0C91E,Negro,M,BIKINITEST2-NEGRO-M,12,/images/vestido_general.webp,60000.00
FE86AD8B-D785-4FDF-9E7B-39D5E8119742,9F60E648-A129-4D75-9404-F7671F9E81B1,Negro,M,BRUNA-NEGRO-M,1,/images/vestido_general.webp,65000.00
C5E421D0-F814-4C02-B30E-3F33525B077E,ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Blanco,L,DAHIAN-BLANCO-L,1,/images/vestido_general.webp,59900.00
D0FAD803-49C7-42AB-B9D2-41496A5419CE,E97160D6-BF84-400C-B8E3-6DB98E737F87,Negro,M,RENATA-NEGRO-M,1,/images/vestido_general.webp,69900.00
173F8DD0-D572-4D2A-BCCE-633EC304C545,D75C8E9C-F8CD-4BC6-B95C-D22A2905D841,Blanco,M,KIMONOARENA-BLANCO-M,1,/images/vestido_general.webp,45500.00
18C3F78C-F44C-4BEE-9FCD-6A08717CD5E7,ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Negro,M,DAHIAN-NEGRO-M,1,/images/vestido_general.webp,59900.00
3B6FEB8A-6C90-430E-B8EB-6C80ABB76AC8,C7821BE9-0385-4818-A5C1-1DEFD7A08C3F,Negro,M,CARMEN-NEGRO-M,1,/images/vestido_general.webp,72500.00
3767030C-43A8-4FC8-A3B1-6DC63BB643D0,C9E8A2DB-C7D0-4F7C-91EE-77DB5C09D2D3,Azul,M,LIA-AZUL-M,1,/images/vestido_general.webp,59900.00
AB2BD463-87E3-4C8F-8354-74DDFFFDB217,2FFD1D7E-08C9-42DC-9A95-FDC003EEECCA,Azul,M,MAIA-AZUL-M,1,/images/vestido_general.webp,59900.00
3AD38098-53E0-4280-9D9F-7C318CEB9FF8,52E96DB1-0CA1-4B79-8F18-FF33525B077E,Blanco,M,SALIDADALIA-BLANCO-M,1,/images/vestido_general.webp,25000.00
6697F1A4-3530-4347-9F36-81A0F26D33BD,2AC8AE15-12E5-46AE-8354-46BB6ADAE710,Blanco,M,ALLIE-BLANCO-M,1,/images/vestido_general.webp,59900.00
0C9DBB80-8369-41E8-A21A-831B77C203BB,66322DC5-3C43-4F39-BFA4-E967EA8D7E75,Negro,M,BIKINITEST-NEGRO-M,1,/images/vestido_general.webp,60000.00
1F40A54B-942C-4372-B60A-8917A39C1A85,69FE73BE-431A-4190-A99C-7F944EAB92A9,Blanco,M,SALIDAANNA-BLANCO-M,1,/images/vestido_general.webp,25000.00
D7550D1D-A104-444F-AB10-8A0BB42D678A,4C16B4F7-E4E4-4337-84CB-E1C9637A2D7D,Blanco,S,ANTONIA-BLANCO-S,1,/images/vestido_general.webp,60000.00
3C3A3CEB-64D6-46AD-BC39-8B1D617651BF,ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Blanco,S,DAHIAN-BLANCO-S,1,/images/vestido_general.webp,59900.00
A83939B8-8A26-475B-955E-932058787E65,77D68A19-BA27-47E7-A516-860FAB6F3B2B,Blanco,M,SALIDALISA-BLANCO-M,1,/images/vestido_general.webp,25000.00
0E3107C6-F5FC-41C2-90A0-949A59F088FA,88D95559-2057-4139-8512-6026D362830C,cafe,M,MAT-CAFE-M,1,/images/vestido_general.webp,50000.00
4EAA6C35-2C08-483F-9438-9B2B8B418946,2F9C8343-921B-4567-BF92-4BBA8ABC649D,Blanco,M,SOBRERCARLA-BLANCO-M,1,/images/vestido_general.webp,25000.00
EEDC3D42-751C-4D03-819E-ACFA744AE82F,C9E37258-B92C-4089-99C6-5CCA88F0EE5D,Negro,M,LORENA-NEGRO-M,1,/images/vestido_general.webp,69900.00
6D7F95B5-DA2E-4F41-84EB-B6103C2C81E7,ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Negro,S,DAHIAN-NEGRO-S,1,/images/vestido_general.webp,59900.00
8CA206CA-A79C-4C3F-9346-C490F4942DC6,EA5CF076-62E8-4A83-94CC-70660CF583AF,Blanco,M,VENTOSA-BLANCO-M,1,/images/vestido_general.webp,69900.00
FD1363BE-93F4-40A3-BABB-C6FCF2923A8A,4C16B4F7-E4E4-4337-84CB-E1C9637A2D7D,Blanco,L,ANTONIA-BLANCO-L,1,/images/vestido_general.webp,60000.00
A8B7AA17-30CB-48CA-9C9F-C993E3B02283,0C8BB861-D5D3-457A-AF43-2ED3F23DEF2D,Verde,M,RANA-VERDE-M,1,/images/vestido_general.webp,59900.00
936C2A6E-6AAF-461A-9512-D4EDD964856D,ED27AEFC-1A3F-4709-9F19-E0A10C6C4224,Blanco,M,DAHIAN-BLANCO-M,1,/images/vestido_general.webp,59900.00
101E3588-E7AF-4CCA-9BB8-D61AB076C2CD,6650CDF9-A85B-4BA3-8EC7-E5F949CD2329,Rojo,M,AMBER-ROJO-M,1,/images/vestido_general.webp,59900.00
8B1EFFA9-D5A9-46AF-BA5F-DB17CB43B138,984F83DB-14FA-4B97-B0DF-749F6630BCC1,Verde,M,PAMELA-VERDE-M,1,/images/vestido_general.webp,59900.00
9D26A6EA-4FCE-45AC-860E-DDD6484D6D53,CBDB7982-561B-423C-9A1A-3FBFC42C4B1A,Negro,M,EMILIA-NEGRO-M,1,/images/vestido_general.webp,69900.00
917244D3-9D93-4FCE-B5CE-E5392F051050,902C0FE2-04A1-4D46-BA96-FF6A17D7B0D9,Negro,M,BIKINITESTT-NEGRO-M,1,/images/vestido_general.webp,55000.00
97C7B819-D1F3-4451-A0A1-E602AECF8268,1E075966-8FDF-4415-83BC-7E7C1A712E9A,Vino,M,LARA-VINO-M,1,/images/vestido_general.webp,60950.00
200ADAC2-E1DE-45EB-8048-ED322BC3A524,103187B4-13D3-49E8-ADD5-D31F1F8C13B7,Negro,M,KIMONOANDREA-NEGRO-M,1,/images/vestido_general.webp,55000.00
EF06D6CD-B04C-4D75-9A8F-F066C074FE50,CD93405D-5A5B-4170-9DCD-1E78E1C3977C,Negro,M,JULIETTE-NEGRO-M,1,/images/vestido_general.webp,69900.00
A5425EF9-D512-45B5-8197-F5061B918C2C,B4AB5517-E93F-4B61-AA76-D54610188B2A,Negro,M,SALIDALIORA-NEGRO-M,1,/images/vestido_general.webp,30000.00
32D5CA2D-8555-45D1-9D56-FC21672C60B1,63AF4702-1CDE-4E7A-97DE-E536E3039997,Negro,M,SALIDADANA-NEGRO-M,1,/images/vestido_general.webp,25000.00
98D91257-5E35-4EB9-9D31-FD90F1AB7DDD,A456E3D5-23D3-4C01-8C05-CFDC56B6EED1,Verde,M,CELESTE-VERDE-M,1,/images/vestido_general.webp,72500.00
`;


// Función genérica para parsear cualquier CSV
function parseCSV(csvString) {
    console.log("Parsing CSV string...");
    const lines = csvString.trim().split('\n');
    if (lines.length === 0) {
        console.warn("CSV string is empty after trimming.");
        return [];
    }
    const headers = lines[0].split(',').map(header => header.trim());
    const data = [];

    for (let i = 1; i < lines.length; i++) {
        const values = lines[i].split(',').map(value => value.trim());
        if (values.length === headers.length) {
            let row = {};
            for (let j = 0; j < headers.length; j++) {
                row[headers[j]] = values[j];
            }
            data.push(row);
        } else {
            console.warn(`Skipping malformed row: "${lines[i]}". Expected ${headers.length} columns, got ${values.length}.`);
        }
    }
    console.log(`Parsed ${data.length} rows.`);
    return data;
}

// Parsear ambos CSVs
const productsData = parseCSV(rawProductsCsvContent);
const productVariantsData = parseCSV(rawProductVariantsCsvContent);

console.log("Products Data:", productsData);
console.log("Product Variants Data:", productVariantsData);

// Función para obtener la imagen por defecto según la categoría
function getDefaultImageUrl(category) {
    const lowerCaseCategory = category ? category.toLowerCase() : '';
    switch (lowerCaseCategory) {
        case 'bikini':
            return '/images/vestido_general.webp';
        case 'enterizo':
            return '/images/enterizo_general.webp';
        case 'bikini set':
            return '/images/bikini_set.webp';
        case 'salidas':
            return '/images/salidas_general.png';
        case 'accesorios':
            return '/images/accesorios_general.png';
        case 'serena':
            return '/images/serene_general.png';
        default:
            return '/images/vestido_general.webp'; // Imagen por defecto si la categoría no coincide
    }
}

// Unir los datos de productos y variantes
const allProducts = productsData.map(product => {
    const variants = productVariantsData.filter(variant => variant.ProductId === product.Id);

    // Determinar la imagen por defecto basada en la categoría del producto
    let imageUrl = getDefaultImageUrl(product.Description);

    // Usar el precio del producto si no hay variantes o si la variante no tiene un precio base válido
    let basePrice = parseFloat(product.BasePrice);

    // Si existen variantes y la primera variante tiene un precio base válido, usarlo
    if (variants.length > 0 && !isNaN(parseFloat(variants[0].BasePrice))) {
        basePrice = parseFloat(variants[0].BasePrice);
    }

    return {
        id: product.Id,
        name: product.Name,
        category: product.Description, // Usamos Description como categoría
        price: basePrice,
        oldPrice: null, // No hay precio anterior en los CSV actuales
        imageUrl: imageUrl, // Usar la imagen por defecto de la categoría
        isPromotion: false, // No hay datos de promoción en los CSV actuales
        isActive: product.IsActive === '1', // Convertir a booleano
        variants: variants // Añadir las variantes al objeto del producto
    };
}).filter(product => product.isActive); // Filtrar solo productos activos

console.log("All Processed Products:", allProducts);


// Función para crear una tarjeta de producto
function createProductCard(product) {
    const card = document.createElement('div');
    card.className = 'product-card';
    card.dataset.id = product.id; // Añadir un ID al dataset para referencia

    // Contenedor de la imagen
    const imageContainer = document.createElement('div');
    imageContainer.className = 'product-image-container';
    const img = document.createElement('img');
    img.src = product.imageUrl; // Ahora usa la imagen por defecto de la categoría
    img.alt = product.name;
    imageContainer.appendChild(img);

    // Etiqueta de promoción (si aplica)
    if (product.isPromotion) {
        const promoTag = document.createElement('span');
        promoTag.className = 'promotion-tag';
        promoTag.textContent = 'Promoción';
        imageContainer.appendChild(promoTag);
    }
    card.appendChild(imageContainer);

    // Información del producto
    const info = document.createElement('div');
    info.className = 'product-info';

    const name = document.createElement('h3');
    name.className = 'product-name';
    name.textContent = product.name;
    info.appendChild(name);

    const prices = document.createElement('div');
    prices.className = 'product-prices';

    if (product.oldPrice) {
        const oldPrice = document.createElement('span');
        oldPrice.className = 'product-old-price';
        oldPrice.textContent = `$${product.oldPrice.toLocaleString('es-CO')}`; // Formato de moneda COP
        prices.appendChild(oldPrice);
    }

    const currentPrice = document.createElement('span');
    currentPrice.className = 'product-current-price';
    currentPrice.textContent = `$${product.price.toLocaleString('es-CO')}`; // Formato de moneda COP
    prices.appendChild(currentPrice);
    info.appendChild(prices);

    // Acciones (botones)
    const actions = document.createElement('div');
    actions.className = 'product-actions';

    const addToCartBtn = document.createElement('button');
    addToCartBtn.className = 'btn btn-primary';
    addToCartBtn.textContent = 'Añadir al Carrito';
    // MODIFICACIÓN CLAVE: Llamar a window.addProductToCart con la información de la variante
    addToCartBtn.addEventListener('click', () => {
        // En este ejemplo, tomamos la primera variante disponible.
        // En un caso real, podemos tener un selector de talla/color.
        const defaultVariant = product.variants && product.variants.length > 0 ? product.variants[0] : null;

        if (defaultVariant) {
            const itemToAdd = {
                id: defaultVariant.Id, // Usar el ID de la variante como ID único del ítem en el carrito
                name: product.name,
                price: parseFloat(defaultVariant.BasePrice),
                imageUrl: product.imageUrl, // Usar la imagen de la categoría para el carrito
                size: defaultVariant.Size,
                color: defaultVariant.Color
            };
            console.log('Añadiendo al carrito:', itemToAdd);
            //  window.addProductToCart definido en cart.js
            if (typeof window.addProductToCart === 'function') {
                window.addProductToCart(itemToAdd);
                alert(`"${itemToAdd.name}" (${itemToAdd.size}, ${itemToAdd.color}) añadido al carrito.`);
                // El contador del carrito se actualizará automáticamente por cart.js's saveCart -> updateCartItemCount
            } else {
                console.error("window.addProductToCart no está definida. Asegúrate de que cart.js se cargue correctamente y exporte la función.");
            }
        } else {
            alert('No hay variantes disponibles para este producto.');
            console.warn('No variants found for product:', product.name);
        }
    });
    actions.appendChild(addToCartBtn);

    const viewDetailsBtn = document.createElement('button');
    viewDetailsBtn.className = 'btn btn-outline-secondary';
    viewDetailsBtn.textContent = 'Ver Detalles';
    viewDetailsBtn.addEventListener('click', () => {
        console.log('Ver detalles de:', product.name);
        // Redirigir a una página de detalles del producto
        // window.location.href = `product-detail.html?id=${product.id}`;
        alert('Funcionalidad "Ver Detalles" no implementada aún.');
    });
    actions.appendChild(viewDetailsBtn);
    info.appendChild(actions);

    card.appendChild(info);
    return card;
}

// Función para renderizar los productos
// Acepta un array de productos opcional para renderizar (usado por la búsqueda)
function renderProducts(category = null, productsToRender = null) {
    const productsContainer = document.getElementById('productsContainer');
    if (!productsContainer) {
        console.error("Error: Element with ID 'productsContainer' not found in the DOM for rendering.");
        return;
    }
    productsContainer.innerHTML = ''; // Limpiar el contenedor antes de añadir nuevos productos

    let productsToDisplay;
    if (productsToRender) {
        // Si se proporciona una lista específica (por ejemplo, desde la búsqueda), úsala
        productsToDisplay = productsToRender;
        console.log(`Rendering ${productsToDisplay.length} products from override list.`);
    } else if (category) {
        // Si hay una categoría, filtra allProducts por categoría
        productsToDisplay = allProducts.filter(p => p.category.toLowerCase() === category.toLowerCase());
        console.log(`Found ${productsToDisplay.length} products for category "${category}".`);
    } else {
        // Si no hay categoría ni lista específica, muestra todos los productos
        productsToDisplay = allProducts;
        console.log(`Found ${productsToDisplay.length} total products.`);
    }

    if (productsToDisplay.length > 0) {
        productsToDisplay.forEach(product => {
            const productCard = createProductCard(product);
            productsContainer.appendChild(productCard);
        });
        console.log(`Successfully rendered ${productsToDisplay.length} product cards.`);
    } else {
        productsContainer.innerHTML = `<p class="text-center">No hay productos disponibles en la categoría "${category || 'todos los productos'}" o que coincidan con la búsqueda.</p>`;
        console.warn(`No products to display for category: ${category || 'All'}.`);
    }
}

// Función para verificar estado de sesión y actualizar UI
function updateAuthUI() {
    const userAccountLink = document.getElementById('userAccountLink');
    const userAccountText = document.getElementById('userAccountText');
    const token = localStorage.getItem('jwtToken');
    const userName = localStorage.getItem('userName'); // Obtenemos el nombre de usuario si lo guardamos

    if (token) {
        console.log("User is logged in. Updating UI.");
        if (userAccountLink) {
            userAccountLink.href = 'profile.html'; // Cambiar enlace a profile.html
        }
        if (userAccountText) {
            userAccountText.textContent = userName ? `Hola, ${userName}` : 'Tu Perfil'; // Mostrar "Hola, [nombre]" o "Tu Perfil"
        }
    } else {
        console.log("User is not logged in. Showing 'Iniciar Sesión'.");
        if (userAccountLink) {
            userAccountLink.href = 'auth.html'; // Asegurarse de que el enlace apunta a auth.html
        }
        if (userAccountText) {
            userAccountText.textContent = 'Iniciar Sesión'; // Mostrar "Iniciar Sesión"
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    console.log("DOM fully loaded and parsed. Initializing product display.");

    // Inicializar el contador del carrito al cargar la página
    if (typeof window.updateCartItemCountDisplay === 'function') {
        window.updateCartItemCountDisplay();
    } else {
        console.warn("window.updateCartItemCountDisplay no está definida en main.js. Asegúrate de que cart.js se cargue primero.");
    }

    // Llama a la función de actualización de UI al cargar la página
    updateAuthUI();

    // Llama a la función para renderizar solo los Bikinis al cargar la página
    renderProducts('Bikini');

    // Lógica para el navbar: filtrar productos al hacer clic en las categorías
    const navItems = document.querySelectorAll('.nav-list .nav-item a');
    navItems.forEach(item => {
        item.addEventListener('click', (event) => {
            event.preventDefault(); // Evita que el enlace recargue la página
            const categoryText = event.target.textContent.trim(); // Obtén el texto y quita espacios

            // Mapea el texto del menú a la categoría real del CSV
            let categoryToFilter = null; // Por defecto null para mostrar todos si el menú es "Tienda"

            if (categoryText === 'Tienda') {
                categoryToFilter = null; // Mostrar todos los productos
            } else if (categoryText === 'Bikinis') {
                categoryToFilter = 'Bikini';
            } else if (categoryText === 'Bikini Sets') {
                categoryToFilter = 'Bikini Set';
            } else if (categoryText === 'Enterizos') {
                categoryToFilter = 'Enterizo';
            } else if (categoryText === 'Salidas') {
                categoryToFilter = 'Salidas';
            } else if (categoryText === 'Accesorios') {
                categoryToFilter = 'Accesorios';
            } else if (categoryText === 'Serena') { // Si "Serena" es una categoría en tu CSV (Description)
                categoryToFilter = 'Serena';
            }
            // Puedes añadir más mapeos si tienes otras categorías en tu menú y CSV

            renderProducts(categoryToFilter);

            // Actualiza la clase 'active' en la navegación
            navItems.forEach(link => link.classList.remove('active'));
            event.target.classList.add('active');

            // Limpiar el campo de búsqueda al cambiar de categoría
            const searchInput = document.getElementById('searchInput');
            if (searchInput) {
                searchInput.value = '';
            }
        });
    });

    // --- Funcionalidad de Búsqueda en Tiempo Real ---
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', (event) => {
            const searchTerm = event.target.value.toLowerCase();
            const filteredProducts = allProducts.filter(product =>
                product.name.toLowerCase().includes(searchTerm) ||
                product.category.toLowerCase().includes(searchTerm)
            );
            renderProducts(null, filteredProducts); // Renderiza los productos filtrados por búsqueda
        });
    }
});
