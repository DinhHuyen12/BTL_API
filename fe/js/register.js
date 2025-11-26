document.getElementById("registerForm").addEventListener("submit", registerUser);

async function registerUser(event) {
    event.preventDefault();

    let username = document.getElementById("username").value.trim();
    let fullName = document.getElementById("fullName").value.trim();
    let email = document.getElementById("email").value.trim();
    let password = document.getElementById("password").value.trim();
    let password2 = document.getElementById("password2").value.trim();
    let agree = document.getElementById("agree").checked;

    if (!agree) {
        iziToast.warning({
            title: "Chưa đồng ý",
            message: "Bạn cần đồng ý điều khoản!",
            position: "topRight"
        });
        return;
    }

    if (password !== password2) {
        iziToast.error({
            title: "Lỗi!",
            message: "Password xác nhận không trùng khớp!",
            position: "topRight"
        });
        return;
    }

    // RoleId mặc định = 1
    let roleId = 1;

    let result = await apiPost("auth/register", {
        Username: username,
        FullName: fullName,
        Email: email,
        PasswordHash: password,
        RoleId: roleId
    });

    console.log("Register response:", result);

    if (result.data?.Success === true) {

        iziToast.success({
            title: "Thành công!",
            message: "Tạo tài khoản thành công!",
            position: "topRight"
        });

        setTimeout(() => {
            window.location.href = "login.html";
        }, 1200);

    } else {

        iziToast.error({
            title: "Đăng ký thất bại",
            message: result.errorMessage || result.message || "Không tạo được tài khoản!",
            position: "topRight"
        });
    }
}
