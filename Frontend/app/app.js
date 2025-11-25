var app = angular.module('myApp', ['ngRoute']);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "app/views/home.html",
            controller: "MainController"
        })
        .when("/login", {
            templateUrl: "app/views/login.html",
            controller: "AuthController"
        })
        .when("/register", {
            templateUrl: "app/views/register.html",
            controller: "AuthController"
        })
        .otherwise({ redirectTo: "/" });
});
