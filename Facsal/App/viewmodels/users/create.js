define(['services/unitofwork', 'services/errorhandler',
    'services/logger'],
    function (uow, errorhandler, logger) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            assignmentVMs: ko.observableArray(),
            roles: ko.observable(),
            user: ko.observable(),

            saveUser: saveUser,
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var self = this,

                roles = unitofwork.roles.all()
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
                return logger.log('No changes were detected.', null, null, true);
            }

            unitofwork.commit()
                .then(function () {
                    return logger.logSuccess('Save successful', null, null, true);
                })
                .fail(self.handleError);

            return true;
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