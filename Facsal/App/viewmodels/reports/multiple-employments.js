define(['services/unitofwork', 'services/errorhandler',
    'services/config'],
    function (uow, errorhandler, config) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            departmentId: ko.observable(),
            persons: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate(departmentId) {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            vm.departmentId(departmentId);

            return true;
        }

        function attached(view) {
            var self = this;

            var persons = unitofwork.personsWithMutipleEmployments.all()
                .then(function (response) {
                    return vm.persons(response);
                });

            Q.all([
                persons
            ]).fail(self.handleError);

            return true;
        }
    });