(function () {

    "use strict";

    angular.module("app")
        .controller("traceRouteController", traceRouteController);

    function traceRouteController($http, $timeout, $scope) {

        var vm = this;

        $(function () {
            
          
        }); 

        vm.TraceRoute = function()
        {
            // add crap in here to happen when button is pressed
            // make api call to our server, GET request using Angular HTTP req
            // /api/trace/google.com
            console.log(vm.Hostname);
        };
    };

})();
