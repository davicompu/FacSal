define(['services/unitofwork', 'services/errorhandler'],
    function (uow, errorhandler) {
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

            departments: ko.observableArray(),
            salaries: ko.observableArray(),
            selectedUnitId: ko.observable(),
            selectedDepartmentId: ko.observable(),
            units: units,
        };

        vm.selectedUnitId.subscribe(function (newValue) {
            var predicate = new breeze.Predicate('unitId', '==', newValue);
            return unitofwork.departments.find(predicate)
                .then(function (response) {
                    vm.departments(response);
                });
        });

        vm.selectedDepartmentId.subscribe(function (newValue) {
            if (newValue === undefined) {
                return vm.salaries([]);
            }

            var predicate = breeze.Predicate.create('person.employments', 'any', 'departmentId', '==', newValue)
                .and('cycleYear', '==', 2014);
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