﻿define([],
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
            employmentsUrl: 'employments',
            facultyTypesUrl: 'facultyTypes',
            lookupsUrl: 'getLookups',
            meritAdjustmentTypesUrl: 'meritAdjustmentTypes',
            personsUrl: 'persons',
            rankTypesUrl: 'rankTypes',
            rolesUrl: 'roles',
            roleAssignmentsUrl: 'roleAssignments',
            salariesUrl: 'salaries',
            specialAdjustmentTypesUrl: 'specialAdjustmentTypes',
            specialSalaryAdjustmentsUrl: 'specialSalaryAdjustments',
            statusTypesUrl: 'statusTypes',
            unitsUrl: 'units',
            usersUrl: 'users',
        };
        //#endregion

        return self;
    });