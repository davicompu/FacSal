define(['services/unitofwork'],
    function (uow) {
        var unitofwork = uow.create(),

            salary = ko.observable(),

            appointmentTypes = ko.observableArray(),

            facultyTypes = ko.observableArray(),

            meritAdjustmentTypes = ko.observableArray(),

            rankTypes = ko.observableArray(),

            specialAdjustmentTypes = ko.observableArray(),

            activate = function (salaryId) {
                var self = this;

                unitofwork.appointmentTypes.all()
                    .then(function (response) {
                        self.appointmentTypes(response);
                    });

                unitofwork.facultyTypes.all()
                    .then(function (response) {
                        self.facultyTypes(response);
                    });

                unitofwork.meritAdjustmentTypes.all()
                    .then(function (response) {
                        self.meritAdjustmentTypes(response);
                    });

                unitofwork.rankTypes.all()
                    .then(function (response) {
                        self.rankTypes(response);
                    });

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