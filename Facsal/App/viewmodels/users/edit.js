define(['services/unitofwork', 'services/errorhandler',
    'services/logger', 'global/session', 'durandal/system',
    'plugins/router'],
    function (uow, errorhandler, logger, session, system,
        router) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,
            deactivate: deactivate,

            assignmentVMs: ko.observableArray(),
            columnLength: ko.observable(4),
            selectedDepartmentId: ko.observable(),
            user: ko.observable(),
            units: ko.observableArray(),
            userId: ko.observable(),

            cancelChanges: cancelChanges,
            saveChanges: saveChanges,
        };

        vm.selectedDepartmentId.subscribe(function (newValue) {
            if (newValue === 'Choose...' || newValue === undefined) {
                return vm.assignmentVMs([]);
            }

            var p1 = new breeze.Predicate('name', breeze.FilterQueryOp.Contains, newValue),

                roles = unitofwork.roles.find(p1)
                    .then(function (response) {
                        var roles = response,

                            user = vm.user(),

                            assignmentHash = createRoleAssignmentHash(user),

                            assignmentMapVMs = $.map(roles, function (role) {
                                return {
                                    role: role,
                                    isSelected: ko.observable(!!assignmentHash[role.id()])
                                };
                            });

                        vm.assignmentVMs(assignmentMapVMs);
                    })
                    .fail(function (response) {
                        logger.logError(response.statusText, response, system.getModuleId(vm), true);
                    });

            return true;
        });

        vm.assignmentVMRows = ko.computed(function () {
            var result = [],
                row,
                colLength = parseInt(vm.columnLength(), 10);

            /* Loop through items and push each into a row array that gets
               pushed into the final result.
            */
            for (var i = 0, j = vm.assignmentVMs().length; i < j; i++) {
                if (i % colLength === 0) {
                    if (row) {
                        result.push(row);
                    }
                    row = [];
                }
                row.push(vm.assignmentVMs()[i]);
            }

            // Push the final row.
            if (row) {
                result.push(row);
            }

            return result;
        });

        errorhandler.includeIn(vm);

        return vm;

        function activate(userId) {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            vm.userId(userId);

            return true;
        }

        function attached() {
            var predicate = new breeze.Predicate('id', '==', vm.userId()),

                expansionCondition = 'roleAssignments',

                user = unitofwork.users.find(predicate, expansionCondition)
                    .then(function (response) {
                        vm.user(response[0]);
                    });

            if (session.userIsInRole('manage-all')) {
                unitofwork.units.all()
                    .then(function (response) {
                        vm.units(response);
                    });
            } else {
                unitofwork.manageableUnits.all()
                    .then(function (response) {
                        vm.units(response);
                    });
            }

            return true;
        }

        function deactivate() {
            vm.selectedDepartmentId(undefined);
            vm.user(undefined);
            vm.units(undefined);
            return true;
        }

        function saveChanges() {
            var self = this;

            applySelectionsToRoleAssignmentMap();

            if (!unitofwork.hasChanges()) {
                return logger.log('No changes were detected.', null, system.getModuleId(vm), true);
            }

            unitofwork.commit()
                .then(function (response) {
                    return logger.logSuccess('Save successful', response, system.getModuleId(vm), true);
                })
                .fail(self.handleError);

            return true;
        }

        function cancelChanges() {
            unitofwork.rollback();
            return router.navigateBack();
        }

        function createRoleAssignmentHash(user) {
            var assignmentHash = {};

            $.each(user.roleAssignments(), function (index, map) {
                assignmentHash[map.roleId()] = map;
            });

            return assignmentHash;
        }

        function applySelectionsToRoleAssignmentMap() {
            var user = vm.user(),
                mapVMs = vm.assignmentVMs(),
                assignmentHash = createRoleAssignmentHash(user);

            $.each(mapVMs, function (index, mapVM) {
                var map = assignmentHash[mapVM.role.id()];

                if (mapVM.isSelected()) {
                    // User selected this assignment.
                    if (!map) {
                        // No existing map, so create one.
                        map = unitofwork.roleAssignments.create({
                            roleId: mapVM.role.id(),
                            userId: user.id()
                        });
                    }
                } else {
                    // User unselected this assignment.
                    if (map) {
                        // If map exists, delete it.
                        map.entityAspect.setDeleted();
                    }
                }
            });
        }
    });