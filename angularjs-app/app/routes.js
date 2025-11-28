app.config(function ($routeProvider) {

    $routeProvider

        // AUTH LAYOUT (no header/sidebar/footer)
        .when("/login", {
            templateUrl: "app/layouts/authLayout.html",
            controller: function ($rootScope) {
                $rootScope.pageTemplate = "app/views/login.html";
            }
        })

        .when("/register", {
            templateUrl: "app/layouts/authLayout.html",
            controller: function ($rootScope) {
                $rootScope.pageTemplate = "app/views/register.html";
            }
        })

        .when("/verify-otp", {
            templateUrl: "app/layouts/authLayout.html",
            controller: function ($rootScope) {
                $rootScope.pageTemplate = "app/views/verifyOtp.html";
            }
        })


        // MAIN LAYOUT (header + sidebar + footer)
        .when("/", {
            templateUrl: "app/layouts/mainLayout.html"
        })

        .when("/admin", {
            templateUrl: "app/layouts/mainLayout.html"
        })

        .otherwise({ redirectTo: "/login" });

});
