app.service('ApiService', function ($http) {
    const BASE_URL = 'https://localhost:7083/api';

    this.get = function (endpoint) {
        return $http.get(`${BASE_URL}/${endpoint}`);
    };

    this.post = function (endpoint, data) {
        return $http.post(`${BASE_URL}/${endpoint}`, data, {
            headers: { 'Content-Type': 'application/json' }
        });
    };
});
