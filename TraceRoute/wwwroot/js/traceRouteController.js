(function () {

    "use strict";

    angular.module("app")
        .controller("traceRouteController", traceRouteController);

    function traceRouteController($http, $timeout, $scope) {

        var vm = this;
        var theResponse;
        var latlongs;
        var i;
        var marker;
        var markers = [];
        var position;
        var path;

        $(function () {
            
          
        }); 

        vm.TraceRoute = function()
        {
            markers = [];
            clearMarkersAndPaths();

            $http.get("api/trace/"+vm.Hostname)
            .then(
                function successFunction(response){
                    theResponse = angular.fromJson(response);
                    latlongs = theResponse.data;

                    for(i = 0; i < latlongs.length; i++){
                        position = {lat: latlongs[i].coordinates.latitude, lng: latlongs[i].coordinates.longitude};
                        marker = addMarker(position, (i+1));
                        markers.push(marker);
                    }
                    autoZoom();
                }

            )
        };
    };

})();
