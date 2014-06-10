define(['services/unitofwork', 'services/errorhandler'],
    function (uow, errorhandler) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            selectedDepartmentId: ko.observable(),
            units: ko.observableArray(),
            users: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        vm.selectedDepartmentId.subscribe(function (newValue) {
            var predicate = new breeze.Predicate(
                'roleAssignments', 'any', 'role.name', '==', 'update-' + newValue);
            var users = unitofwork.users.find(predicate)
                .then(function (response) {
                    vm.users(response);
                });

            Q.all([users]).fail(self.handleError);

            return true;
        });

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var self = this;

            var units = unitofwork.units.all()
                .then(function (response) {
                    vm.units(response);
                });

            Q.all([units]).fail(self.handleError);

            return true;
        }
    });