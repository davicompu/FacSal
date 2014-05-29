define([],
    function () {

        //#region Public interface.
        var self = {
            // Authentication Routes.
            siteUrl: "/",
            userInfoUrl: "/api/account/userinfo",

            remoteServiceName: '/breeze/data',
            // Breeze Routes. Relative to remote service name.
            departmentsUrl: 'departments',
            lookupUrl: 'getlookups',
            personsUrl: 'persons',
            salariesUrl: 'salaries',
            unitsUrl: 'units',
        };
        //#endregion

        return self;
    });