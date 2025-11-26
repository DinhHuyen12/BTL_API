document.getElementById("forgotForm").addEventListener("submit", forgotPassword);

async function forgotPassword(event) {
    event.preventDefault();

    let email = document.getElementById("email").value.trim();

    if (!email) {
        iziToast.warning({
            title: "Thiếu email",
            message: "Vui lòng nhập email cần reset mật khẩu!",
            position: "topRight"
        });
        return;
    }

    let result = await apiPost("user/forgot-password", {
        email: email
    });

    console.log("Forgot password response:", result);

    if (result.data?.Success === true) {

        iziToast.success({
            title: "Thành công!",
            message: result.data.Message || "Đã gửi email reset mật khẩu",
            position: "topRight"
        });

        // Sau 1.2s quay lại login
        setTimeout(() => {
            window.location.href = "reset-password.html";
        }, 1200);

    } else {

        iziToast.error({
            title: "Lỗi!",
            message: result.message || "Không thể gửi email reset mật khẩu!",
            position: "topRight"
        });
    }
}
