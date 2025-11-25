app.directive('appHeader', function () {
    return {
        restrict: 'E',
        templateUrl: 'app/layouts/header.html',
        controller: 'LayoutController'
    };
});
