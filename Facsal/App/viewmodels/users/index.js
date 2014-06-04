﻿define(['services/unitofwork', 'services/errorhandler'],
    function (uow, errorhandler) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,

            departments: ko.observableArray(),
            units: ko.observableArray(),
            users: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        vm.selectedUnitId.subscribe(function (newValue) {
            var predicate = new breeze.Predicate(
                'unitId', '==', newValue);
            return unitofwork.departments.find(predicate)
                .then(function (response) {
                    vm.departments(response);
                });
        });

        vm.selectedDepartmentId.subscribe(function (newValue) {
            var predicate = new breeze.Predicate(
                'roleAssignments', 'any', 'role.name', 'update-' + newValue)
            return unitofwork.users.find(predicate)
                .then(function (response) {
                    vm.users(response);
                });
        })

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var units = unitofwork.units.all()
                .then(function (response) {
                    vm.units(response);
                });
        }
    });