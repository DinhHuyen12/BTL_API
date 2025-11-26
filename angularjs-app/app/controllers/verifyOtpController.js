app.controller("verifyOtpController", function ($scope, $location, AuthService) {

    console.log("verifyOtpController LOADED!");

    $scope.model = {
        email: sessionStorage.getItem("emailVerify"),
        code: ""
    };

    console.log("Loaded model:", $scope.model);

    $scope.verifyOtp = function () {

        console.log("verifyOtp() FUNCTION CALLED!");
        console.log("Data gửi lên API:", $scope.model);

        AuthService.verifyOtp($scope.model)
            .then(function (res) {

                console.log("OTP API Response:", res.data);

                if (res.data.message && res.data.message.includes("thành công")) {

                    iziToast.success({
                        title: "Success",
                        message: "OTP Verified!",
                        position: "topRight"
                    });

                    sessionStorage.removeItem("emailVerify");
                    $location.path("/");
                    $scope.$applyAsync();
                    // setTimeout(() => {
                    //     window.location = "#!/";
                    // }, 800);

                } else {
                    iziToast.error({
                        title: "Error",
                        message: res.data.message || "OTP incorrect!",
                        position: "topRight"
                    });
                }

            })
            .catch(function (err) {

                console.error("Verify OTP Error:", err);

                iziToast.error({
                    title: "Server Error",
                    message: "Cannot call API!",
                    position: "topRight"
                });
            });
    };

});
