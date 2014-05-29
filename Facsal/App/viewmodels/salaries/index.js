define(['services/unitofwork'],
    function (uow) {
        var unitofwork = uow.create(),

            units = ko.observableArray(),
            
            attached = function (view) {
                var self = this;

                var units = unitofwork.units.all()
                    .then(function (response) {
                        self.units(response);
                    });

                Q.all([units]).fail(self.handleError);
            };

        var vm = {
            activate: activate,
            attached: attached,
            departments: ko.observableArray(),
            persons: ko.observableArray(),
            selectedUnitId: ko.observable(),
            selectedDepartmentId: ko.observable(),
            units: units,
        };

        vm.selectedUnitId.subscribe(function (newValue) {
            var predicate = new breeze.Predicate('unitId', '==', newValue);
            return vm.departments(unitofwork.departments.findInCache(predicate));
        });

        vm.selectedDepartmentId.subscribe(function (newValue) {
            var predicate = new breeze.Predicate('')
        })

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
        }
    });