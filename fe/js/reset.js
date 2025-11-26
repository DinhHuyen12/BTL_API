// document.getElementById("resetForm").addEventListener("submit", resetPassword);

// async function resetPassword(event) {
//     event.preventDefault();

//     const token = document.getElementById("token").value.trim();
//     const password = document.getElementById("password").value.trim();
//     const password2 = document.getElementById("password2").value.trim();

//     if (!token) {
//         iziToast.warning({
//             title: "Thiếu token",
//             message: "Vui lòng nhập token!",
//             position: "topRight"
//         });
//         return;
//     }

//     if (!password || !password2) {
//         iziToast.warning({
//             title: "Thiếu mật khẩu",
//             message: "Vui lòng nhập mật khẩu mới!",
//             position: "topRight"
//         });
//         return;
//     }

//     if (password !== password2) {
//         iziToast.error({
//             title: "Sai mật khẩu",
//             message: "Mật khẩu xác nhận không trùng khớp!",
//             position: "topRight"
//         });
//         return;
//     }

//     // Gửi API
//     let result = await apiPost("user/reset-password", {
//         token: token,
//         newPassword: password
//     });

//     console.log("Reset Password:", result);

//     if (result.data?.Success === true) {

//         iziToast.success({
//             title: "Thành công",
//             message: "Mật khẩu đã được đặt lại!",
//             position: "topRight"
//         });

//         setTimeout(() => {
//             window.location.href = "login.html";
//         }, 1200);

//     } else {

//         iziToast.error({
//             title: "Lỗi",
//             message: result.message || "Không thể đặt lại mật khẩu!",
//             position: "topRight"
//         });

//     }
// }







// ===========================
// TỰ ĐỘNG LẤY TOKEN TRONG LINK
// ===========================
function getTokenFromUrl() {
    let params = new URLSearchParams(window.location.search);
    return params.get("token");
}

// Nếu có token trong URL → tự điền vào input
let urlToken = getTokenFromUrl();
if (urlToken) {
    document.getElementById("token").value = urlToken;
}

document.getElementById("resetForm").addEventListener("submit", resetPassword);

async function resetPassword(event) {
    event.preventDefault();

    const token = document.getElementById("token").value.trim();
    const password = document.getElementById("password").value.trim();
    const password2 = document.getElementById("password2").value.trim();

    if (!token) {
        iziToast.warning({
            title: "Thiếu token",
            message: "Token không hợp lệ hoặc đã hết hạn!",
            position: "topRight"
        });
        return;
    }

    if (!password || !password2) {
        iziToast.warning({
            title: "Thiếu mật khẩu",
            message: "Vui lòng nhập mật khẩu mới!",
            position: "topRight"
        });
        return;
    }

    if (password !== password2) {
        iziToast.error({
            title: "Sai mật khẩu",
            message: "Mật khẩu xác nhận không trùng khớp!",
            position: "topRight"
        });
        return;
    }

    // Gọi API reset password
    let result = await apiPost("user/reset-password", {
        token: token,
        newPassword: password
    });

    console.log("Reset Password API:", result);

    if (result.data?.Success === true) {

        iziToast.success({
            title: "Thành công!",
            message: "Mật khẩu đã được đặt lại!",
            position: "topRight"
        });

        setTimeout(() => {
            window.location.href = "login.html";
        }, 1200);

    } else {

        iziToast.error({
            title: "Lỗi!",
            message: result.message || "Không thể đặt lại mật khẩu!",
            position: "topRight"
        });
    }
}
