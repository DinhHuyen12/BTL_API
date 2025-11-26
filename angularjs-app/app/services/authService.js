app.service("AuthService", function (ApiService) {

    this.login = function (data) {
        return ApiService.post("auth/login-step1", data);
    };

    this.verifyOtp = function (data) {
        return ApiService.post("auth/verify-otp", data);
    };

});
