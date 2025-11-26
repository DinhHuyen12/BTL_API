app.controller('AuthController', function ($scope, AuthService, $window, $rootScope) {
    $scope.user = {};

    $scope.login = function () {

        AuthService.login($scope.user)
            .then(function (res) {

                console.log("Login response:", res.data);

                // KIỂM TRA ĐÚNG MESSAGE API TRẢ VỀ
                if (res.data.message &&
                    res.data.message.includes("OTP") &&
                    res.data.email) {

                    // Lưu email để verify otp
                    sessionStorage.setItem("emailVerify", res.data.email);

                    iziToast.success({
                        title: "Đăng nhập bước 1 thành công!",
                        message: "OTP đã được gửi đến email.",
                        position: "topRight"
                    });

                    // CHUYỂN TRANG
                    setTimeout(function () {
                        $window.location.href = "#!/verify-otp";
                    }, 1000);

                } else {
                    iziToast.error({
                        title: "Lỗi",
                        message: res.data.message || "Không thể đăng nhập",
                        position: "topRight"
                    });
                }

            })
            .catch(function (err) {
                console.error("Login error:", err);
                iziToast.error({
                    title: "Lỗi đăng nhập",
                    message: "Sai thông tin hoặc server lỗi!",
                    position: "topRight"
                });
            });
    };

    // Register
    $scope.register = function () {
        AuthService.register($scope.user)
            .then(function (res) {
                console.log(res.data);

                iziToast.success({
                    title: 'Thành công',
                    message: 'Đăng ký thành công!',
                    position: 'topRight',
                    timeout: 2000,
                    progressBar: true,
                });

                $scope.user = {};
                if ($scope.registerForm) {
                    $scope.registerForm.$setPristine();
                    $scope.registerForm.$setUntouched();
                }

                setTimeout(function () {
                    $window.location.href = "#!/login";
                }, 2000);
            })
            .catch(function (err) {
                console.error("Register error:", err);

                iziToast.error({
                    title: 'Lỗi đăng ký',
                    message: err.data?.message || 'Đăng ký thất bại!',
                    position: 'topRight',
                    timeout: 3000,
                    progressBar: true,
                });
            });
    };
});