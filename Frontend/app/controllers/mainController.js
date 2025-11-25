app.controller('MainController', function ($scope, ApiService) {
    $scope.message = "Xin chào từ trang chủ!";

    // Gọi thử API backend
    ApiService.get('hello').then(res => {
        $scope.message = res.data.message || $scope.message;
    });
});
