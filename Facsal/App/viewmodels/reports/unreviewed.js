﻿define(['services/unitofwork', 'services/errorhandler',
    'services/config'],
    function (uow, errorhandler, config) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            deactivate: deactivate,

            departmentData: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate(unitId, departmentId) {
            var self = this;

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
                    'toLower(unitId)', '==', unitId.toLowerCase()),

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

        function getData(departments) {
            var self = this;

            return $.each(departments, function (index, department) {
                var p1 = breeze.Predicate.create(
                    'person.employments', 'any', 'departmentId', '==', department.id()),
                    p2 = breeze.Predicate.create(
                    'cycleYear', '==', config.currentCycleYear),
                    p3 = breeze.Predicate.create(
                    'meritAdjustmentTypeId', '==', null),
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