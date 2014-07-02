define(['services/unitofwork', 'services/errorhandler',
    'services/logger', 'services/config', 'durandal/system',
    'plugins/router'],
    function (uow, errorhandler, logger, config, system,
        router) {

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

            cancelPerson: cancelPerson,
            savePerson: savePerson,
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached() {
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
                return logger.log('No changes were detected.', null, system.getModuleId(vm), true);
            }

            unitofwork.commit()
                .then(function (response) {
                    logger.logSuccess('Save successful', response, system.getModuleId(vm), true);
                    return router.navigateBack();
                })
                .fail(self.handleError);

            return true;
        }

        function cancelPerson() {
            unitofwork.rollback();
            return router.navigateBack();
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