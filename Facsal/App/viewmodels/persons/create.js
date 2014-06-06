define(['services/unitofwork', 'services/errorhandler',
    'services/logger'],
    function (uow, errorhandler, logger) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            person: ko.observable(),
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
                
                statusTypes = unitofwork.statusTypes.all()
                    .then(function (response) {
                        vm.statusTypes(response);
                    }),

                units = unitofwork.units.all()
                    .then(function (response) {
                        vm.units(response);
                    });

            vm.person(unitofwork.persons.create());

            Q.all([statusTypes, units]).fail(self.handleError);

            return true;
        }

        function savePerson() {
            var self = this;

            applySelectionsToEmploymentCollection();

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