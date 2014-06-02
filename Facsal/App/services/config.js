define([],
    function () {

        //#region Public interface.
        var self = {
            // Authentication Routes.
            siteUrl: "/",
            userInfoUrl: "/api/account/userinfo",

            remoteServiceName: '/breeze/data',
            // Breeze Routes. Relative to remote service name.
            appointmentTypesUrl: 'appointmentTypes',
            departmentsUrl: 'departments',
            facultyTypesUrl: 'facultyTypes',
            lookupsUrl: 'getLookups',
            meritAdjustmentTypesUrl: 'meritAdjustmentTypes',
            personsUrl: 'persons',
            rankTypesUrl: 'rankTypes',
            salariesUrl: 'salaries',
            specialAdjustmentTypesUrl: 'specialAdjustmentTypes',
            unitsUrl: 'units',
        };
        //#endregion

        return self;
    });