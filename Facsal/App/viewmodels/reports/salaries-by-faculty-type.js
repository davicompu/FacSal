define(['services/unitofwork', 'services/errorhandler',
    'services/config'],
    function (uow, errorhandler, config) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            departmentId: ko.observable(),
            salaries: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate(departmentId) {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            vm.departmentId(departmentId);

            return true;
        }

        function attached(view) {
            var salaries = unitofwork.salariesByFacultyType(vm.departmentId())
                .then(function (response) {
                    return vm.salaries(response);
                });

            Q.all([
                salaries
            ]).fail(self.handleError);

            return true;
        }
    });