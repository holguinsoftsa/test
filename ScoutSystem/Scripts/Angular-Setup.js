//Constant
var ERROR_MESSAGE = "Failed to load data. Please refresh or contact an administrator.";

//Angular Setup
var app = angular.module('locationhub', ['720kb.datepicker', 'ngAnimate', 'ui.select', 'ngSanitize'], function ($locationProvider) {
    $locationProvider.html5Mode({ enabled: true, requireBase: false });
});