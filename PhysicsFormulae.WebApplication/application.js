﻿var application = angular.module("PhysicsFormulae", ["ngRoute", "ngSanitize"]);

application.config(function ($routeProvider) {
    $routeProvider
        .when("/", { templateUrl: "search.html", controller: "SearchController" })
        .when("/tag/:tagName", { templateUrl: "search.html", controller: "SearchController" })
        .when("/field/:fieldName", { templateUrl: "search.html", controller: "SearchController" })
        .when("/formula/:reference", { templateUrl: "formula.html", controller: "FormulaController" })
        .when("/constant/:reference", { templateUrl: "constant.html", controller: "ConstantController" })
        .when("/about", { templateUrl: "about.html" });
});

application.directive("mathematics", function () {
    return {
        restrict: "E",
        link: function (scope, element, attributes) {
            var contentType = attributes.contentType;
            var content = attributes.content;
            if (typeof (katex) === "undefined") {
                require(["katex"], function (katex) {
                    katex.render(content, element[0]);
                });
            }
            else {
                katex.render(content, element[0]);
            }
        }
    }
});

application.directive("compile", ["$compile", function ($compile) {
    return function (scope, element, attributes) {
        scope.$watch(function (scope) {
            return scope.$eval(attributes.compile);
        }, function (value) {
            element.html(value);
            $compile(element.contents())(scope);
        });
    };
}]);

application.directive("seeMore", function () {
    return {
        restrict: "E",
        templateUrl: "see-more.html",
        scope: { links: "=links" }
    };
});

application.factory("dataService", ["$http", function ($http) {
    var dataService = {
        getData: function () {
            return $http.get("formulae.json").then(function (response) {
                return response.data;
            });
        }
    };

    return dataService;
}]);

application.service("metaService", function () {
    var title = "Physics Formulae";
    var metaDescription = "";
    var metaKeywords = "";

    return {
        set: function (newTitle, newMetaDescription, newMetaKeywords) {
            title = newTitle;
            metaDescription = newMetaDescription;
            metaKeywords = newMetaKeywords;
        },
        metaTitle: function () { return title; },
        metaDescription: function () { return metaDescription; },
        metaKeywords: function () { return metaKeywords; }
    }
});
