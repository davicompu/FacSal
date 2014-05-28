define(['services/unitofwork'],
    function (uow) {
        var unitofwork = uow.create(),

            units = ko.observableArray(),

            departments = ko.observableArray(),

            persons = ko.observableArray(),
            
            attached = function (view) {
                var self = this;

                var units = unitofwork.units.all()
                    .then(function (response) {
                        self.units(response);
                    });

                var depts = unitofwork.departments.all()
                    .then(function (response) {
                        self.departments(response);
                    });

                Q.all([units, departments]).fail(self.handleError);
            };

        var vm = {
            activate: activate,
            departments: departments,
            persons: persons,
            units: units,
        };

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
        }
    });