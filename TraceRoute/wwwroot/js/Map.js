// Write your JavaScript code.

    var map;  
    var markers = [];  
  
    function initMap() {  
        var lat_lng = {lat: 42.361145, lng: -71.057083};  
  
        map = new google.maps.Map(document.getElementById('map'), {  
        zoom: 12,  
        center: lat_lng,  
        mapTypeId: google.maps.MapTypeId.TERRAIN  
    });  
  
    // This event listener will call addMarker() when the map is clicked.  
    map.addListener('click', function(event) {  
        addMarker(event.latLng);  
    });  
  
    // Adds a marker at the center of the map.  
    addMarker(lat_lng);  
    }  
  
    // Adds a marker to the map and push to the array.  
    function addMarker(location) {  
        var marker = new google.maps.Marker({  
        position: location,  
        map: map  
    });  
    markers.push(marker);  
    }  