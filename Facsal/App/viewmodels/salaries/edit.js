define(['services/unitofwork', 'services/errorhandler', 'services/logger'],
    function (uow, errorhandler, logger) {
        var unitofwork = uow.create(),

            salary = ko.observable(),

            appointmentTypes = ko.observableArray(),

            facultyTypes = ko.observableArray(),

            meritAdjustmentTypes = ko.observableArray(),

            rankTypes = ko.observableArray(),

            specialAdjustmentTypes = ko.observableArray(),
                
            statusTypes = ko.observableArray();

        var vm = {
            activate: activate,
            attached: attached,

            appointmentTypes: appointmentTypes,
            facultyTypes: facultyTypes,
            meritAdjustmentTypes: meritAdjustmentTypes,
            rankTypes: rankTypes,
            salary: ko.observable(),
            salaryId: ko.observable(),
            selectedSpecialAdjustmentTypes: ko.observableArray(),
            specialAdjustmentTypes: specialAdjustmentTypes,
            statusTypes: statusTypes,

            saveChanges: saveChanges,
        };

        vm.errors = ko.validation.group(vm);

        errorhandler.includeIn(vm);

        return vm;

        function activate(salaryId) {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            vm.salaryId(salaryId);
            return true;
        }

        function attached(view) {
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

            var predicate = new breeze.Predicate('id', '==', vm.salaryId());
            var expansionProperties = 'person, specialSalaryAdjustments';
            var salary = unitofwork.salaries.find(predicate, expansionProperties)
                .then(function (response) {
                    vm.salary(response[0]);
                });

            var specialAdjustmentTypes = unitofwork.specialAdjustmentTypes.all()
                .then(function (response) {
                    self.specialAdjustmentTypes(response);
                });

            var statusTypes = unitofwork.statusTypes.all()
                .then(function (response) {
                    self.statusTypes(response);
                });

            Q.all([
                appointmentTypes,
                facultyTypes,
                meritAdjustmentTypes,
                rankTypes,
                salary,
                specialAdjustmentTypes,
                statusTypes
            ]).fail(self.handleError);

            return true;
        }

        function saveChanges() {
            var self = this;

            evaluateSelectedSpecialAdjustmentTypesArray();

            if (!unitofwork.hasChanges()) {
                return true;
            }

            unitofwork.commit()
                .then(function () {
                    logger.logSuccess('Save successful', null, null, true);
                })
                .fail(self.handleError);

            return true;
        }

        function evaluateSelectedSpecialAdjustmentTypesArray() {
            $.each(vm.selectedSpecialAdjustmentTypes(), function (index, value) {
                var adjustment = unitofwork.specialSalaryAdjustments.create({
                    salaryId: vm.salaryId(),
                    specialAdjustmentTypeId: value
                });
            });
        }
    });