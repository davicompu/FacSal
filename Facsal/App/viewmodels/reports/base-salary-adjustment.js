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
            var self = this;

            var p1 = breeze.Predicate.create(
                'salary.cycleYear', '==', config.currentCycleYear),
                p2 = breeze.Predicate.create(
                'salary.person.employments', 'any', 'departmentId', '==', vm.departmentId())
                predicate = breeze.Predicate.and([p1, p2]),
                expansionCondition = 'salary, salary.person';

            var salaries = unitofwork.baseSalaryAdjustments.find(predicate, expansionCondition)
                .then(function (response) {
                    vm.salaries([]);
                    return $.each(response, function (index, value) {
                        return vm.salaries.push(value.salary);
                    });
                });

            Q.all([
                salaries
            ]).fail(self.handleError);

            return true;
        }
    });