const API_BASE = "https://localhost:7083/api/auth/";

// ===============================
// GET API KHÔNG DÙNG BODY/QUERY
// ===============================
async function apiGet(url) {
    const token = localStorage.getItem("token");

    const response = await fetch(API_BASE + url, {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + token,
        }
    });

    const text = await response.text();
    if (!text) {
        return { success: false, message: "Empty server response" };
    }

    try {
        return JSON.parse(text);
    } catch (err) {
        console.error("Invalid JSON from server:", text);
        return { success: false, message: "Invalid JSON", raw: text };
    }
}


// ===============================
// KIỂM TRA TOKEN KHI LOAD TRANG
// ===============================
document.addEventListener("DOMContentLoaded", async function () {

    let token = localStorage.getItem("token");

    if (!token) {
        window.location.href = "login.html";
        return;
    }

    console.log("Validating token...");

    let result = await apiGet("validate-token");

    console.log("Validate token result:", result);

    if (!result.success) {
        iziToast.error({
            title: "Hết hạn",
            message: "Phiên đăng nhập đã hết hạn!",
            position: "topRight"
        });

        localStorage.clear();
        setTimeout(() => window.location.href = "login.html", 800);
        return;
    }

    // Lưu user vào localStorage
    localStorage.setItem("user", JSON.stringify(result.user));

    // Hiển thị user lên header
    renderUserHeader(result.user);
});


function renderUserHeader(user) {

    // 1. Update the short name next to avatar
    document.querySelectorAll(".nav-link-user span").forEach(el => {
        el.textContent = user.fullName;
    });

    // 2. Update dropdown title "Hello ..."
    const dropdownTitle = document.querySelector(".dropdown-title");
    if (dropdownTitle) {
        dropdownTitle.textContent = "Hello " + user.fullName;
    }

    // 3. Update dropdown menu for logged-in user
    let menu = document.querySelector(".dropdown-menu-right");

    if (menu) {
        menu.innerHTML = `
            <div class="dropdown-title">Hello ${user.fullName}</div>

            <a href="profile.html" class="dropdown-item has-icon">
                <i class="far fa-user"></i> Profile
            </a>

            <a href="#" class="dropdown-item has-icon">
                <i class="fas fa-cog"></i> Settings
            </a>

            <div class="dropdown-divider"></div>

            <a href="#" onclick="logout()" class="dropdown-item has-icon text-danger">
                <i class="fas fa-sign-out-alt"></i> Logout
            </a>
        `;
    }
}



// LOGOUT
function logout() {
    localStorage.clear();

    iziToast.success({
        title: "Đăng xuất",
        message: "Hẹn gặp lại!",
        position: "topRight"
    });

    setTimeout(() => window.location.href = "login.html", 700);
}
