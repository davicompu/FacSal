define(['services/unitofwork', 'services/errorhandler',
    'services/config'],
    function (uow, errorhandler, config) {
        var unitofwork = uow.create(),

            units = ko.observableArray(),
            
            attached = function (view) {
                var self = this;

                var units = unitofwork.units.all()
                    .then(function (response) {
                        self.units(response);
                    });

                Q.all([units]).fail(self.handleError);

                return true;
            };

        var vm = {
            activate: activate,
            attached: attached,

            salaries: ko.observableArray(),
            selectedDepartmentId: ko.observable(),
            units: units,
        };

        vm.selectedDepartmentId.subscribe(function (newValue) {
            if (newValue === 'Select a department...' || newValue === undefined) {
                return vm.salaries([]);
            }

            var predicate = breeze.Predicate
                .create('person.employments', 'any', 'departmentId', '==', newValue)
                .and('cycleYear', '==', config.currentCycleYear);
            var expansionProperty = 'person';

            return unitofwork.salaries.find(predicate, expansionProperty)
                .then(function (response) {
                    vm.salaries(response);
                });
        });

        errorhandler.includeIn(vm);

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }
    });