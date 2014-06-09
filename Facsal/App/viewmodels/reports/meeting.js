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
            var p1 = breeze.Predicate.create(
                'person.employments', 'any', 'departmentId', '==', vm.departmentId()),
                p2 = breeze.Predicate.create(
                'cycleYear', '==', config.currentCycleYear),
                predicate = breeze.Predicate.and([p1, p2]),
                expansionCondition = 'person';

            var salaries = unitofwork.salaries.find(predicate, expansionCondition)
                .then(function (response) {
                    return vm.salaries(response);
                });

            Q.all([
                salaries
            ]).fail(self.handleError);

            return true;
        }
    });