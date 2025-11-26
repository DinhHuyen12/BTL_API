const API_BASE = "https://localhost:7083/api/";

async function apiPost(endpoint, data) {
    const res = await fetch(API_BASE + endpoint, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    return res.json();
}
