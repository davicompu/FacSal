define([],
    function () {

        //#region Public interface.
        var self = {
            // Authentication Routes.
            siteUrl: "/",
            userInfoUrl: "/api/account/userinfo",

            remoteServiceName: '/breeze/data',
            // Breeze Routes. Relative to remote service name.
            lookupUrl: 'getlookups',
            adjustmentTypeUrl: '',
            appointmentTypeUrl: '',
            departmentUrl: '',
            facultyTypeUrl: '',
            personUrl: '',
            rankTypeUrl: '',
            salaryUrl: '',
            unitUrl: '',
        };
        //#endregion

        return self;
    });