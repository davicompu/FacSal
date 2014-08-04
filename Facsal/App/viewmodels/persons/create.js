define(['global/session', 'services/errorhandler',
    'services/logger', 'services/config', 'durandal/system',
    'plugins/router'],
    function (session, errorhandler, logger, config, system,
        router) {

        var unitofwork = session.unitofwork();

        var vm = {
            activate: activate,
            attached: attached,

            appointmentTypes: ko.observableArray(),
            facultyTypes: ko.observableArray(),
            leaveTypes: ko.observableArray(),
            person: ko.observable(),
            rankTypes: ko.observableArray(),
            salary: ko.observable(),
            statusTypes: ko.observableArray(),
            selectedDepartmentId: ko.observable(),

            cancelPerson: cancelPerson,
            savePerson: savePerson,
        };

        vm.selectedDepartmentId.subscribe(function (newValue) {
            if (newValue === 'Select a department...' || newValue === undefined) {
                return;
            }

            initializeEmployment();
        })

        errorhandler.includeIn(vm);

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached() {

            $('html,body').animate({ scrollTop: 0 }, 0);

            var self = this,
                
                appointmentTypes = unitofwork.appointmentTypes.all()
                    .then(function (response) {
                        vm.appointmentTypes(response);
                    }),

                facultyTypes = unitofwork.facultyTypes.all()
                    .then(function (response) {
                        vm.facultyTypes(response);
                    }),

                leaveTypes = unitofwork.leaveTypes.all()
                    .then(function (response) {
                        vm.leaveTypes(response);
                    }),

                rankTypes = unitofwork.rankTypes.all()
                    .then(function (response) {
                        vm.rankTypes(response);
                    }),

                statusTypes = unitofwork.statusTypes.all()
                    .then(function (response) {
                        vm.statusTypes(response);
                    });

            vm.person(unitofwork.persons.create());

            vm.salary(unitofwork.salaries.create({
                personId: vm.person().id(),
                cycleYear: config.currentCycleYear,
                meritAdjustmentTypeId: config.meritAdjustmentTypeIdIndicatesNotReviewed
            }));

            vm.errors = ko.validation.group([
                vm.person().pid,
                vm.person().firstName,
                vm.person().lastName
            ]);

            Q.all([
                appointmentTypes,
                facultyTypes,
                leaveTypes,
                rankTypes,
                statusTypes
            ]).fail(self.handleError);

            return true;
        }

        function savePerson() {
            var self = this;

            if (vm.errors().length !== 0) {
                vm.errors.showAllMessages();
                logger.logError('Errors detected.', null, system.getModuleId(vm), true);
                return;
            }

            vm.person().salary = vm.salary();

            if (!unitofwork.hasChanges()) {
                return logger.log('No changes were detected.', null, system.getModuleId(vm), true);
            }

            unitofwork.commit()
                .then(function (response) {
                    logger.logSuccess('Save successful', response, system.getModuleId(vm), true);
                    router.navigate('/employments/edit/' + vm.person().id())
                })
                .fail(self.handleError);

            return true;
        }

        function cancelPerson() {
            unitofwork.rollback();
            return router.navigateBack();
        }
    });