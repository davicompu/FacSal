define(['services/unitofwork'],
    function (uow) {
        var unitofwork = uow.create(),

            salary = ko.observable(),

            appointmentTypes = ko.observableArray(),



            activate = function (salaryId) {
                var self = this;

                return unitofwork.salaries.withId(salaryId)
                    .then(function (response) {
                        self.salary(response);
                    });
            };

        var vm = {
            activate: activate,
            salary: salary,
        };

        return vm;
    });