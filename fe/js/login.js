document.querySelector("form").addEventListener("submit", login);

async function login(event) {
    event.preventDefault();

    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();

    if (!username || !password) {
        iziToast.warning({
            title: 'Thiếu thông tin',
            message: 'Vui lòng nhập username và password!',
            position: 'topRight'
        });
        return;
    }

    try {
        const result = await apiPost("Auth/login", {
            username: username,
            password: password
        });

        console.log("Login response:", result);

        if (result.success === true) {

            iziToast.success({
                title: 'OTP đã gửi!',
                message: 'Mã OTP đã gửi đến email: ' + result.email,
                position: 'topRight'
            });

            // Lưu email để verify OTP
            localStorage.setItem("verifyEmail", result.email);

            setTimeout(() => {
                window.location.href = "verify-otp.html";
            }, 1000);

        } else {

            iziToast.error({
                title: 'Đăng nhập thất bại',
                message: result.message || "Username hoặc mật khẩu không đúng.",
                position: 'topRight'
            });

        }

    } catch (err) {

        console.error("Login error:", err);

        iziToast.error({
            title: 'Lỗi server',
            message: 'Không thể kết nối backend!',
            position: 'topRight'
        });
    }
}
