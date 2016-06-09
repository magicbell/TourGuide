
var routeEditorController = function ($scope, $http, $location, $routeParams) {
    $scope.route = $routeParams.routeName;
    $scope.points = [];
    $scope.newPoint = {};
    $scope.wikiPoints = [];
    $scope.paramsClass = "ng-hide";
    $scope.wikiRadius = 2;
    $scope.errorMessage = "";
    $scope.language = "en";

    var controlUrl = $location.protocol() + "://" + $location.host();
    if ($location.port() != null)
        controlUrl += ":" + $location.port();
    controlUrl += "/api/routes/" + $scope.route + "/points";

    $scope.isBusy = true;

    showPoints();

    function _showMap(points) {
        if (points && points.length > 0) {
            var mapPoints = _.map(points, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });

            var latitudes = [];
            var longitudes = [];
            var ind = 0;
            angular.forEach(points, function (point) {
                longitudes[ind] = point.longitude;
                latitudes[ind] = point.latitude;
                ind++;
            });

            var maxDistance = Math.max((Math.max.apply(null, longitudes) - Math.min.apply(null, longitudes)), (Math.max.apply(null, latitudes) - Math.min.apply(null, latitudes)));
            var zoom = 0;
            if (maxDistance <= 1) zoom = 9;
            if (maxDistance > 1 && maxDistance <= 2) zoom = 8;
            if (maxDistance > 2 && maxDistance <= 5) zoom = 7;
            if (maxDistance > 5 && maxDistance <= 10) zoom = 6;
            if (maxDistance > 10 && maxDistance <= 75) zoom = 5;
            if (maxDistance > 75 && maxDistance <= 100) zoom = 4;
            if (maxDistance > 100 && maxDistance < 160) zoom = 3;
            if (maxDistance > 160) zoom = 2;

            //Show Map
            travelMap.createMap({
                stops: mapPoints,
                selector: "#map",
                //currentStop: 1,
                initialZoom: zoom
            });
        }
    };

    function showPoints() {
        $http.get(controlUrl)
    .then(function (responce) {
        angular.copy(responce.data, $scope.points);

        _showMap($scope.points);
    }, function (error) {
        $scope.errorMessage = "Failed to load data: " + error;
    })
    .finally(function () {
        $scope.isBusy = false;
    });
    };

    $scope.addPoint = function () {

        $scope.errorMessage = "";
        $scope.isBusy = true;

        $http.post(controlUrl, $scope.newPoint)
        .then(function (responce) {
            // $scope.points.push(responce.data);
            showPoints();
            // _showMap($scope.points);
        }, function (error) {
            $scope.errorMessage = "Failed to save new point " + error;
        })
        .finally(function () {
            $scope.isBusy = false;
            $scope.newPoint = {};
        });
    };

    $scope.deletePoint = function (route, pointId) {

        $scope.errorMessage = "";
        $scope.isBusy = true;

        $http.delete(controlUrl + "/" + pointId)
        .then(function (responce) {
            // $scope.points.push(responce.data);
            showPoints();
            // _showMap($scope.points);
        }, function (error) {
            $scope.errorMessage = "Failed to delete point " + error;
        })
        .finally(function () {
            $scope.isBusy = false;
        });
    }

    $scope.upPoint = function (route, point) {

        $scope.errorMessage = "";

        var index = $scope.points.indexOf(point);

        if (index > 0) {
            $scope.isBusy = true;
            var pointId1 = point.id;
            var pointId2 = $scope.points[index - 1].id;
            $http.put(controlUrl + "/" + pointId1 + "/" + pointId2)
            .then(function (responce) {
                showPoints();
            }, function (error) {
                $scope.errorMessage = "Failed to swap points " + error;
            })
            .finally(function () {
                $scope.isBusy = false;
            });
        }
        else
            $scope.errorMessage = "Can't up location because it's the first one.";
    }

    $scope.downPoint = function (route, point) {

        $scope.errorMessage = "";

        var index = $scope.points.indexOf(point);

        if (index < $scope.points.length - 1) {
            $scope.isBusy = true;
            var pointId1 = point.id;
            var pointId2 = $scope.points[index + 1].id;
            $http.put(controlUrl + "/" + pointId1 + "/" + pointId2)
            .then(function (responce) {
                showPoints();
            }, function (error) {
                $scope.errorMessage = "Failed to swap points " + error;
            })
            .finally(function () {
                $scope.isBusy = false;
            });
        }
        else
            $scope.errorMessage = "Can't down location because it's the last one.";
    }

    $scope.showWikiParams = function () {
        if ($scope.paramsClass == "ng-hide")
            $scope.paramsClass = "ng-show";
        else $scope.paramsClass = "ng-hide";
    }

    function findWikiById(wikiId) {
        var result = false;
        angular.forEach($scope.wikiPoints, function(point){
            if (point.pageID == wikiId)
                result = true;
        })
        return result;
    }

    $scope.showWikiPoints = function () {
        $scope.errorMessage = "";
        $scope.isBusy = true;
        $scope.paramsClass = "ng-hide";
        $scope.wikiPoints = [];
        //$scope.wikiParams.latitude = $scope.points[0].latitude;
        //$scope.wikiParams.longitude = $scope.points[0].longitude;
        //var wp = {
        //    "radius" : $scope.wikiParams.radius,
        //    "latitude" : $scope.wikiParams.latitude,
        //    "longitude" : $scope.wikiParams.longitude
        //}

        /* $http.get(controlUrl + "/wiki/" + $scope.wikiParams.radius)
         .then(function (responce) {
 
             // $scope.points.push(responce.data);
             showPoints();
             // _showMap($scope.points);
         }, function (error) {
             $scope.errorMessage = "Failed to save new point " + error;*/

        angular.forEach($scope.points, function (point) {
            var wikipediaURL = 'http://' + $scope.language + '.wikipedia.org/w/api.php?action=query&list=geosearch&gsradius=' + $scope.wikiRadius * 1000 + '&gscoord=' + point.latitude + '|' + point.longitude + '&prop=extracts|info|pageimages&exintro&format=json&explaintext&redirects&inprop=url&indexpageids&export&format=json&callback=JSON_CALLBACK';

            $http.jsonp(wikipediaURL)
                            .then(function (response3) {
                                try {
                                    var pageID = response3.data.query.pageids[0];
                                }
                                catch (e) {
                                    $scope.errorMessage = "Failed to load wiki data " + e.message;
        }
            },
                            function (error) {
                                $scope.errorMessage = "Failed to load wiki data " + error;
                            })
                                    .finally(function () {
                                        $scope.isBusy = false;
                                    });



            /*$http.jsonp('https://wdq.wmflabs.org/api?q=AROUND[625,' + point.latitude + ',' + point.longitude + ',' + $scope.wikiRadius + ']&callback=JSON_CALLBACK')
            .then(function (response) {
                try {
                    var items = response.data.items;
                }
                catch (e) {
                    $scope.errorMessage = "Failed to load wiki data " + e.message;
                }
                //  $scope.jason = items;
                angular.forEach(items, function (item) {
                    var wikiDataString = 'http://www.wikidata.org/w/api.php?action=wbgetentities&format=json&ids=Q' + item + '&props=sitelinks%7Csitelinks%2Furls&callback=JSON_CALLBACK';
                    $http.jsonp(wikiDataString)
                    .then(function (response2) {
                        //$scope.jason2 = response2;
                        var url = "";
                        try {
                            url = response2.data.entities["Q" + item].sitelinks[$scope.language+"wiki"].url;
                        }
                        catch (e) {
                            //$scope.errorMessage = "Failed to load wiki data " + e.message;
                        }
                        if (url != "") {
                            var wikipediaTitle = url.substr(30, url.length);
                            var wikipediaURL = 'http://'+$scope.language+'.wikipedia.org/w/api.php?action=query&prop=extracts|info&exintro&titles=' + wikipediaTitle + '&format=json&explaintext&redirects&inprop=url&indexpageids&format=json&callback=JSON_CALLBACK';
                            //var wikipediaURL = 'https://en.wikipedia.org/w/api.php?action=query&prop=revisions&rvprop=content&format=jsonfm&titles=' + wikipediaTitle + '&callback=JSON_CALLBACK';
                            $http.jsonp(wikipediaURL)
                            .then(function (response3) {
                                try {
                                    var pageID = response3.data.query.pageids[0];
                                    if (!findWikiById(pageID)) {
                                        var query = response3.data.query;

                                        var title = response3.data.query.pages[pageID].title;
                                        var fullurl = response3.data.query.pages[pageID].fullurl;
                                        var content = response3.data.query.pages[pageID].extract;

                                        var wikiPoint = {
                                            pageID: pageID,
                                            title: title,
                                            content: content,
                                            fullurl: fullurl,
                                            jason: query
                                        };
                                        $scope.wikiPoints.push(wikiPoint);
                                    }
                                }
                                catch (e) {
                                    $scope.errorMessage = "Failed to load wiki data " + e.message;
                                }
                            },
                            function (error) {
                                $scope.errorMessage = "Failed to load wiki data " + error;
                            });
                        }
                    },
                    function (error) {
                        $scope.errorMessage = "Failed to load wiki data " + error;
                    })
                }, function (error) {
                    $scope.errorMessage = "Failed to load wiki data " + error;
                })            
            }).finally(function () {
                $scope.isBusy = false;
            });*/
        });
    }
}