document.getElementById("verifyEmail").innerText = localStorage.getItem("verifyEmail");

document.getElementById("otpForm").addEventListener("submit", verifyOtp);

async function verifyOtp(event) {
    event.preventDefault();

    const code = document.getElementById("otp").value.trim();
    const email = localStorage.getItem("verifyEmail");

    if (!code) {
        iziToast.warning({
            title: 'Thiếu OTP',
            message: 'Vui lòng nhập mã OTP!',
            position: 'topRight'
        });
        return;
    }

    try {
        const result = await apiPost("auth/verify-otp", {
            email: email,
            code: code
        });

        console.log("Verify OTP:", result);

        if (result.success === true) {

            iziToast.success({
                title: 'Đăng nhập thành công',
                message: 'OTP chính xác — đang chuyển hướng...',
                position: 'topRight'
            });

            // Lưu token & user vào localStorage
            localStorage.setItem("token", result.token);
            localStorage.setItem("user", JSON.stringify(result.user));

            // Xóa email OTP
            localStorage.removeItem("verifyEmail");

            // Chuyển về trang Home
            setTimeout(() => {
                window.location.href = "index.html";
            }, 900);

        } else {

            iziToast.error({
                title: 'OTP sai',
                message: result.message || "OTP không hợp lệ!",
                position: 'topRight'
            });

        }

    } catch (err) {

        console.error("Verify error:", err);

        iziToast.error({
            title: 'Lỗi server',
            message: 'Không thể kết nối đến backend!',
            position: 'topRight'
        });
    }
}
