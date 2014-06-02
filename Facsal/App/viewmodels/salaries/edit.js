define(['services/unitofwork', 'services/errorhandler'],
    function (uow, errorhandler) {
        var unitofwork = uow.create(),

            salary = ko.observable(),

            appointmentTypes = ko.observableArray(),

            facultyTypes = ko.observableArray(),

            meritAdjustmentTypes = ko.observableArray(),

            rankTypes = ko.observableArray(),

            specialAdjustmentTypes = ko.observableArray(),

            activate = function (salaryId) {
                ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
                var self = this;

                var appointmentTypes = unitofwork.appointmentTypes.all()
                    .then(function (response) {
                        self.appointmentTypes(response);
                    });

                var facultyTypes = unitofwork.facultyTypes.all()
                    .then(function (response) {
                        self.facultyTypes(response);
                    });

                var meritAdjustmentTypes = unitofwork.meritAdjustmentTypes.all()
                    .then(function (response) {
                        self.meritAdjustmentTypes(response);
                    });

                var rankTypes = unitofwork.rankTypes.all()
                    .then(function (response) {
                        self.rankTypes(response);
                    });

                var salary = unitofwork.salaries.withId(salaryId)
                    .then(function (response) {
                        self.salary(response);
                    });

                Q.all([appointmentTypes, facultyTypes, meritAdjustmentTypes, rankTypes,
                    salary
                ]).fail(self.handleError);

                return true;
            };

        var vm = {
            activate: activate,

            appointmentTypes: appointmentTypes,
            facultyTypes: facultyTypes,
            meritAdjustmentTypes: meritAdjustmentTypes,
            rankTypes: rankTypes,
            specialAdjustmentTypes: specialAdjustmentTypes,
            salary: salary,
        };

        errorhandler.includeIn(vm);

        return vm;
    });