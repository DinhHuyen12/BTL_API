app.controller('AuthController', function ($scope, AuthService, $window) {
    $scope.user = {};

    // Hàm login
    $scope.login = function () {
        AuthService.login($scope.user)
            .then(function (res) {
                // Giả sử backend trả về token
                console.log("Login successful:", res.data);

                // Lưu token vào localStorage nếu có
                if (res.data.token) {
                    localStorage.setItem('token', res.data.token);
                }

                alert("Login thành công!");
                // Redirect sang home
                $window.location.href = "/";
            })
            .catch(function (err) {
                console.error("Login error:", err);
                alert("Đăng nhập thất bại! Kiểm tra username/password.");
            });
    };

    $scope.register = function () {
        AuthService.register($scope.user)
            .then(function (res) {
                alert("Register successful!");
                console.log("Response:", res.data);
                // Redirect sang login nếu muốn
                // $window.location.href = "#!/login";
            })
            .catch(function (err) {
                console.error("Register error:", err);
                alert("Register failed!");
            });
    };
});
