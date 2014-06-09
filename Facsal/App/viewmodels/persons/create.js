﻿define(['services/unitofwork', 'services/errorhandler',
    'services/logger', 'services/config'],
    function (uow, errorhandler, logger, config) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            appointmentTypes: ko.observableArray(),
            facultyTypes: ko.observableArray(),
            person: ko.observable(),
            rankTypes: ko.observableArray(),
            salary: ko.observable(),
            statusTypes: ko.observableArray(),
            selectedDepartmentIds: ko.observableArray(),
            units: ko.observableArray(),

            savePerson: savePerson,
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var self = this,
                
                appointmentTypes = unitofwork.appointmentTypes.all()
                    .then(function (response) {
                        vm.appointmentTypes(response);
                    }),

                facultyTypes = unitofwork.facultyTypes.all()
                    .then(function (response) {
                        vm.facultyTypes(response);
                    }),

                rankTypes = unitofwork.rankTypes.all()
                    .then(function (response) {
                        vm.rankTypes(response);
                    }),

                statusTypes = unitofwork.statusTypes.all()
                    .then(function (response) {
                        vm.statusTypes(response);
                    }),

                units = unitofwork.units.all()
                    .then(function (response) {
                        vm.units(response);
                    });

            vm.person(unitofwork.persons.create());

            vm.salary(unitofwork.salaries.create({
                personId: vm.person().id(),
                cycleYear: config.currentCycleYear
            }));

            Q.all([
                appointmentTypes,
                facultyTypes,
                rankTypes,
                statusTypes,
                units
            ]).fail(self.handleError);

            return true;
        }

        function savePerson() {
            var self = this;

            applySelectionsToEmploymentCollection();

            vm.person().salary = vm.salary();

            if (!unitofwork.hasChanges()) {
                return logger.log('No changes were detected.', null, null, true);
            }

            unitofwork.commit()
                .then(function () {
                    return logger.logSuccess('Save successful', null, null, true);
                })
                .fail(self.handleError);

            return true;
        }

        function applySelectionsToEmploymentCollection() {
            var person = vm.person();

            $.each(vm.selectedDepartmentIds(), function (index, departmentId) {
                unitofwork.employments.create({
                    departmentId: departmentId,
                    personId: person.id()
                });
            });
        }
    });