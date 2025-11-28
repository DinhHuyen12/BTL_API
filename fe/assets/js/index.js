var app = angular.module("myApp", []);

app.controller("displayBook", function ($scope, $http) {
    $scope.books = [];

    $http.get("https://localhost:7083/api/Book/get-all")
        .then(function (response) {
            $scope.books = response.data;
        })
        .catch(function (error) {
            console.error("Lỗi khi lấy dữ liệu sách:", error);
        });

});
