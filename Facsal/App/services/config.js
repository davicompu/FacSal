define([],
    function () {

        //#region Public interface.
        var self = {
            // Authentication Routes.
            siteUrl: "/",
            userInfoUrl: "/api/account/userinfo",

            remoteServiceName: '/breeze/data',
            // Breeze Routes. Relative to remote service name.
            departmentsUrl: '/breeze/data/departments',
            lookupUrl: 'getlookups',
            unitsUrl: '/breeze/data/units',
        };
        //#endregion

        return self;
    });