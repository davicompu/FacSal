﻿define(['services/unitofwork', 'services/errorhandler',
    'services/config'],
    function (uow, errorhandler, config) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            deactivate: deactivate,

            departmentSalaries: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate(unitId, departmentId) {
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

        function getData(departments) {
            return $.each(departments, function (index, department) {
                var p1 = breeze.Predicate.create(
                    'person.employments', 'any', 'departmentId', '==', department.id()),
                    p2 = breeze.Predicate.create(
                    'cycleYear', '==', config.currentCycleYear),
                    predicate = breeze.Predicate.and([p1, p2]),
                    expansionCondition = 'person';

                var salaries = unitofwork.salaries.find(predicate, expansionCondition)
                    .then(function (response) {
                        return vm.departmentSalaries.push({
                            department: department,
                            salaries: response
                        });
                    });

                Q.all([
                    salaries
                ]).fail(self.handleError);
            });
        }

        function deactivate() {
            vm.departmentSalaries([]);

            return true;
        }
    });