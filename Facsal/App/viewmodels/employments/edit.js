define(['global/session', 'services/logger', 'plugins/router',
    'durandal/system'],
    function (session, logger, router, system) {

        var unitofwork = session.unitofwork(),

            vm = {
                activate: activate,
                attached: attached,
                deactivate: deactivate,

                columnLength: ko.observable(3),
                employments: ko.observableArray(),
                employmentVMs: ko.observableArray(),
                personId: ko.observable(),
                units: ko.observableArray(),

                cancelChanges: cancelChanges,
                saveChanges: saveChanges,
            };

        return vm;

        function activate(personId) {
            ga('send', 'pageview',
                { 'page': window.location.href, 'title': document.title });

            vm.personId(personId);

            return true;
        }

        function attached() {
            $('html,body').animate({ scrollTop: 0 }, 0);


            unitofwork.units.all()
                .then(function (response) {
                    vm.units(response);

                    var units = response,

                        p1 = new breeze.Predicate('personId', '==', vm.personId());

                    unitofwork.employments.find(p1)
                        .then(function (response) {
                            var employmentHash = createEmploymentHash(response),

                                employmentMapVMs = $.map(units, function (unit) {
                                    var result = [],
                                        row,
                                        colLength = parseInt(vm.columnLength(), 10);

                                    /* Loop through items and push each into a row array that gets
                                       pushed into the final result.
                                    */
                                    for (var i = 0, j = unit.departments().length; i < j; i++) {
                                        if (i % colLength === 0) {
                                            if (row) {
                                                result.push(row);
                                            }
                                            row = [];
                                        }
                                        row.push({
                                            department: unit.departments()[i],
                                            isSelected: ko.observable(!!employmentHash[unit.departments()[i].id()])
                                        });
                                    }

                                    // Push the final row.
                                    if (row) {
                                        result.push(row);
                                    }

                                    return {
                                        unit: unit,
                                        departmentRows: result
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

        function cancelChanges() {
            unitofwork.rollback();
            return router.navigateBack();
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
                var mapVMDepartmentRows = unitMapVM.departmentRows;

                $.each(mapVMDepartmentRows, function (index, mapVMRow) {

                    $.each(mapVMRow, function (index, mapVM) {
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
            });
        }
    });