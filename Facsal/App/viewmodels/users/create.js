define(['services/unitofwork', 'services/errorhandler',
    'services/logger', 'plugins/router', 'durandal/system'],
    function (uow, errorhandler, logger, router, system) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            assignmentVMs: ko.observableArray(),
            columnLength: ko.observable(4),
            roles: ko.observable(),
            user: ko.observable(),

            cancelUser: cancelUser,
            saveUser: saveUser,
        };

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

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var self = this,

                roles = unitofwork.getAssignableRoles()
                    .then(function (response) {
                        vm.roles(response);

                    assignmentMapVMs = $.map(vm.roles(), function (role) {
                        return {
                            role: role,
                            isSelected: ko.observable(false)
                        };
                    });

                    vm.assignmentVMs(assignmentMapVMs);
                });

            vm.user(unitofwork.users.create());

            Q.all([
                roles
            ]).fail(self.handleError);

            return true;
        }

        function saveUser() {
            var self = this;

            applySelectionsToRoleAssignmentMap();

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

        function cancelUser() {
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
                var map = assignmentHash[mapVM.role.id];

                if (mapVM.isSelected()) {
                    // User selected this assignment.
                    if (!map) {
                        // No existing map, so create one.
                        map = unitofwork.roleAssignments.create({
                            roleId: mapVM.role.id,
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