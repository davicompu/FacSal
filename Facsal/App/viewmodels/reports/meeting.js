﻿define(['global/session', 'services/errorhandler',
    'services/config'],
    function (session, errorhandler, config) {

        var unitofwork = session.unitofwork();

        var vm = {
            activate: activate,
            deactivate: deactivate,

            departmentData: ko.observableArray(),
            departmentId: ko.observable(),
            unitId: ko.observable(),

            downloadExcel: dowloadExcel,
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate(unitId, departmentId) {
            var self = this;

            vm.unitId(unitId);
            vm.departmentId(departmentId);

            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            if (departmentId) {
                var department = unitofwork.departments.withId(departmentId)
                    .then(function (response) {
                        getData([response.entity]);
                    });

                Q.all([
                    department
                ]).fail(self.handleError);
            } else {
                var predicate = breeze.Predicate.create(
                    'toLower(unitId)', '==', unitId),

                    departments = unitofwork.departments.find(predicate)
                        .then(function (response) {
                            getData(response);
                        });

                Q.all([
                    departments
                ]).fail(self.handleError);
            }

            return true;
        }

        function deactivate() {
            vm.departmentData([]);

            return true;
        }

        function dowloadExcel() {
            if (vm.departmentId()) {
                window.location.assign('/ReportFile/DepartmentMeeting/' + vm.departmentId());
            } else if (vm.unitId()) {
                window.location.assign('/ReportFile/UnitMeeting/' + vm.unitId());
            }
        }

        function getData(departments) {
            var self = this;

            return $.each(departments, function (index, department) {
                var p1 = breeze.Predicate.create(
                    'person.employments', 'any', 'departmentId', '==', department.id()),

                    p2 = breeze.Predicate.create(
                    'cycleYear', '==', config.currentCycleYear),

                    p3 = breeze.Predicate.create(
                    'person.statusTypeId', '==', config.activeStatusTypeId),

                    predicate = breeze.Predicate.and([p1, p2, p3]),
                    expansionCondition = 'person';

                var salaries = unitofwork.salaries.find(predicate, expansionCondition)
                    .then(function (response) {
                        return vm.departmentData.push({
                            department: department,
                            salaries: response
                        });
                    });

                Q.all([
                    salaries
                ]).fail(self.handleError);
            });
        }
    });