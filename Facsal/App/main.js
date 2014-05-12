requirejs.config({
    baseUrl: "/App",
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    }
});

define('jquery', function () { return jQuery; });
define('knockout', ko);

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'services/appsecurity'],
    function (app, viewLocator, system, appsecurity) {
        //>>excludeStart("build", true);
        system.debug(true);
        //>>excludeEnd("build");

        app.title = 'FacSal';

        // Specify plugins to install and their configuration.
        app.configurePlugins({
            router: true
        });

        app.start().then(function () {

            // Replace 'viewmodels' in the moduleId with 'views' to locate the view.
            // Look for partial views in a 'views' folder in the root.
            viewLocator.useConvention();

            // Configure knockout validation.
            ko.validation.init({
                decorateElement: true,
                errorElementClass: 'error',
                errorMessageClass: 'error',
                registerExtenders: true,
                messagesOnModified: true,
                insertMessages: true,
                parseInputAttributes: true
            });

            // Configure toastr.js messages.
            toastr.options.timeOut = 0;
            toastr.options.extendedTimeOut = 0;
            toastr.options.closeButton = true;

            // Show the app by setting the root view model for the application.
            appsecurity.initializeAuth()
                .then(function (data) {
                    app.setRoot('viewmodels/shell');
                });
        });
    });