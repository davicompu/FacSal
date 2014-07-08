define(['services/unitofwork', 'services/logger', 'plugins/router',
    'durandal/system'],
    function (uow, logger, router, system) {
        var unitofwork = uow.create(),

            vm = {
                activate: activate,
                attached: attached,
                deactivate: deactivate,

                employments: ko.observableArray(),
                employmentVMs: ko.observableArray(),
                personId: ko.observable(),

                saveChanges: saveChanges,
            };

        return vm;

        function activate(personId) {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            vm.personId(personId);

            return true;
        }

        function attached() {
            $('html,body').animate({ scrollTop: 0 }, 0);


            unitofwork.units.all()
                .then(function (response) {
                    var units = response,

                        p1 = new breeze.Predicate('personId', '==', vm.personId());

                    unitofwork.employments.find(p1)
                        .then(function (response) {
                            var employmentHash = createEmploymentHash(response),

                                employmentMapVMs = $.map(units, function (unit) {
                                    return {
                                        unit: unit,
                                        departments: $.map(unit.departments(), function (department) {
                                            return {
                                                department: department,
                                                isSelected: ko.observable(!!employmentHash[department.id()])
                                            }
                                        })
                                    }
                                });

                            vm.employments(response);
                            vm.employmentVMs(employmentMapVMs);
                        });
                });

            return true;
        }

        function deactivate() {
            vm.employments(undefined);
            vm.employmentVMs(undefined);
        }

        function saveChanges() {
            applySelectionsToEmploymentMap();

            if (!unitofwork.hasChanges()) {
                return logger.log('No changes were detected.', null, system.getModuleId(vm), true);
            }

            return unitofwork.commit()
                .then(function (response) {
                    logger.logSuccess('Save successful', response, system.getModuleId(vm), true);
                    return router.navigateBack();
                });
        }

        function createEmploymentHash(employments) {
            var employmentHash = {};

            $.each(employments, function (index, value) {
                employmentHash[value.department().id()] = value;
            });

            return employmentHash;
        }

        function applySelectionsToEmploymentMap() {
            var employments = vm.employments(),

                unitMapVMs = vm.employmentVMs(),

                employmentHash = createEmploymentHash(employments);

            $.each(unitMapVMs, function (index, unitMapVM) {
                var mapVMs = unitMapVM.departments;

                $.each(mapVMs, function (index, mapVM) {
                    var map = employmentHash[mapVM.department.id()];

                    if (mapVM.isSelected()) {
                        // User selected this employment.
                        if (!map) {
                            // No existing map, so create one.
                            map = unitofwork.employments.create({
                                personId: vm.personId(),
                                departmentId: mapVM.department.id()
                            });
                        }
                    } else {
                        // User unselected this adjustment.
                        if (map) {
                            // If map exists, delete it.
                            map.entityAspect.setDeleted();
                        }
                    }
                });
            });
        }
    });