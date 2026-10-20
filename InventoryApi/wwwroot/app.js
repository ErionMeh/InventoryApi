let products = [];
let orderItems = []; 

async function loadProducts() {
    const response = await fetch("/api/Products");
    products = await response.json();

    const tbody = document.querySelector("#productsTable tbody");
    tbody.innerHTML = "";
    const select = document.getElementById("productSelect");
    select.innerHTML = "";

    for (const p of products) {
        const row = tbody.insertRow();
        row.insertCell().textContent = p.id;
        row.insertCell().textContent = p.name;
        row.insertCell().textContent = p.sku;
        row.insertCell().textContent = p.price;
        row.insertCell().textContent = p.quantityInStock;

        const option = document.createElement("option");
        option.value = p.id;
        option.textContent = `${p.name} (${p.quantityInStock} in stock)`;
        select.appendChild(option);
    }
}

function addItem() {
    const select = document.getElementById("productSelect");
    const productId = Number(select.value);
    const quantity = Number(document.getElementById("quantityInput").value);

    if (!productId || quantity < 1) {
        showMessage("Choose a product and a quantity of at least 1.", true);
        return;
    }

    const name = products.find(p => p.id === productId).name;
    orderItems.push({ productId, name, quantity });
    showMessage("", false);
    renderOrderItems();
}

function renderOrderItems() {
    const list = document.getElementById("orderItems");
    list.innerHTML = "";
    for (const item of orderItems) {
        const li = document.createElement("li");
        li.textContent = `${item.name} x ${item.quantity}`;
        list.appendChild(li);
    }
}

async function placeOrder() {
    if (orderItems.length === 0) {
        showMessage("Add at least one item first.", true);
        return;
    }

    const body = {
        items: orderItems.map(i => ({ productId: i.productId, quantity: i.quantity }))
    };

    const response = await fetch("/api/Orders", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    });

    if (response.ok) {
        showMessage("Order placed!", false);
        orderItems = [];
        renderOrderItems();
        await loadProducts();
    } else {
        const text = await response.text();
        let message = text;
        try {
            const data = JSON.parse(text);
            message = data.message || data.title || text;
        } catch { }
        showMessage(message, true);
    }
}

function showMessage(text, isError) {
    const p = document.getElementById("message");
    p.textContent = text;
    p.style.color = isError ? "red" : "green";
}

document.getElementById("addItemBtn").addEventListener("click", addItem);
document.getElementById("placeOrderBtn").addEventListener("click", placeOrder);

loadProducts();