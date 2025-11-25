app.service('AuthService', function (ApiService) {

    // Login API
    this.login = function (user) {
        return ApiService.post('auth/login', {
            username: user.username,
            password: user.password
        });
    };

    // Register API
    this.register = function (user) {
        return ApiService.post('auth/register', {
            username: user.username,
            passwordHash: user.password,
            fullName: user.fullName,
            email: user.email,
            roleId: user.roleId || 1
        });
    };
});
