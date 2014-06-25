define([],
    function () {

        //#region Public interface.
        var self = {
            currentCycleYear: 2014,
            logOutCounterSeconds: 1170,
            highPercentIncreaseThreshold: 0.08,
            lowPercentIncreaseThreshold: 0.0175,

            // Authentication Routes.
            siteUrl: "/",
            userInfoUrl: "/api/account/getUserInfo",

            remoteServiceName: '/breeze/data',
            // Breeze Routes. Relative to remote service name.
            appointmentTypesUrl: 'appointmentTypes',
            baseSalaryAdjustmentsUrl: 'baseSalaryAdjustments',
            departmentNamesForPerson: '/api/person/getDepartmentNames',
            departmentsUrl: 'departments',
            employmentsUrl: 'employments',
            extendSessionUrl: '/api/session/extend',
            facultyTypesUrl: 'facultyTypes',
            getAssignableRolesUrl: '/api/role/getAssignableRoles',
            logOutUrl: '/cas/logOut',
            lookupsUrl: 'getLookups',
            meritAdjustmentTypesUrl: 'meritAdjustmentTypes',
            personsUrl: 'persons',
            personsWithMultipleEmploymentsUrl:
                '/api/report/getPersonsWithMultipleEmployments',
            rankTypesUrl: 'rankTypes',
            rolesUrl: 'roles',
            roleAssignmentsUrl: 'roleAssignments',
            salariesUrl: 'salaries',
            salariesByFacultyTypeUrl: '/api/report/getSalariesByFacultyType',
            specialAdjustmentTypesUrl: 'specialAdjustmentTypes',
            specialSalaryAdjustmentsUrl: 'specialSalaryAdjustments',
            statusTypesUrl: 'statusTypes',
            unitsUrl: 'units',
            usersByDepartment: '/api/user/getByDepartmentalAccess',
            usersUrl: 'users',
        };
        //#endregion

        return self;
    });