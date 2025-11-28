// Kiểm tra trạng thái đăng nhập
window.onload = function () {
    if (localStorage.getItem("token")) {
        showDashboard();
    } else {
        showLogin();
    }
};

function showLogin() {
    document.getElementById("login-page").style.display = "block";
    document.getElementById("dashboard-page").style.display = "none";
}

function showDashboard() {
    document.getElementById("login-page").style.display = "none";
    document.getElementById("dashboard-page").style.display = "block";

    // Load dữ liệu giả lập
    document.getElementById("c1").innerText = 1287;
    document.getElementById("c2").innerText = 258;
    document.getElementById("c3").innerText = "48,697,000đ";
}

// Hàm đăng nhập
function login() {
    let user = document.getElementById("user").value;
    let pass = document.getElementById("pass").value;

    if (user === "admin" && pass === "123456") {
        localStorage.setItem("token", "logined");
        showDashboard();
    } else {
        alert("Sai tài khoản hoặc mật khẩu!");
    }
}

// Đăng xuất
function logout() {
    localStorage.removeItem("token");
    showLogin();
}
