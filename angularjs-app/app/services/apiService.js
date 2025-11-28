app.service("ApiService", function ($http) {

    var baseUrl = "https://localhost:7083/api/";

    this.post = function (endpoint, data) {
        return $http.post(baseUrl + endpoint, data);
    };

});
