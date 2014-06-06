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

                        //employmentVMs = $.map(vm.units().departments, function (department) {
                        //    return {
                        //        department: department,
                        //        isSelected: ko.observable(false)
                        //    };
                        //});

                        //vm.employmentVMs(employmentVMs);
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

        function createEmploymentHash(person) {
            var employmentHash = {};

            $.each(person.employments(), function (index, map) {
                employmentHash[map.departmentId()] = map;
            });

            return employmentHash;
        }

        function applySelectionsToEmploymentMap() {
            var person = vm.person(),
                mapVMs = vm.employmentVMs(),
                employmentHash = createEmploymentHash(person);

            $.each(mapVMs, function (index, mapVM) {
                var map = employmentHash[mapVM.department.id()];

                if (mapVM.isSelected()) {
                    // User selected this employment.
                    if (!map) {
                        // No existing map, so create one.
                        map = unitofwork.employments.create({
                            departmentId: mapVM.department.id(),
                            personId: person.id()
                        });
                    }
                } else {
                    // User unselected this employment.
                    if (map) {
                        // If map exists, delete it.
                        map.entityAspect.setDeleted();
                    }
                }
            });
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