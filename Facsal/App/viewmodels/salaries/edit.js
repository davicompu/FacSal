﻿define(['services/unitofwork', 'services/errorhandler', 'services/logger'],
    function (uow, errorhandler, logger) {
        var unitofwork = uow.create(),

            salary = ko.observable(),

            appointmentTypes = ko.observableArray(),

            facultyTypes = ko.observableArray(),

            meritAdjustmentTypes = ko.observableArray(),

            rankTypes = ko.observableArray(),

            adjustmentTypes = ko.observableArray(),

            statusTypes = ko.observableArray(),

            adjustmentItemVM = {
                id: ko.observable(),
                name: ko.observable(),
                isSelected: ko.observable()
            };

        var vm = {
            activate: activate,
            attached: attached,

            adjustmentVMs: ko.observableArray(),
            appointmentTypes: appointmentTypes,
            facultyTypes: facultyTypes,
            meritAdjustmentTypes: meritAdjustmentTypes,
            rankTypes: rankTypes,
            salary: ko.observable(),
            salaryId: ko.observable(),
            adjustmentTypes: adjustmentTypes,
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

            var adjustmentTypes = unitofwork.specialAdjustmentTypes.all()
                .then(function (response) {
                    self.adjustmentTypes(response);
                });

            var statusTypes = unitofwork.statusTypes.all()
                .then(function (response) {
                    self.statusTypes(response);
                });

            var predicate = new breeze.Predicate('id', '==', vm.salaryId());
            var expansionProperties = 'person, specialSalaryAdjustments';
            var salary = unitofwork.salaries.find(predicate, expansionProperties)
                .then(function (response) {
                    var salary = response[0];

                    var adjustmentHash = createSalaryAdjustmentHash(salary);

                    var adjustmentMapVMs = $.map(self.adjustmentTypes(), function (adjustment) {
                        return {
                            adjustment: adjustment,
                            isSelected: ko.observable(!!adjustmentHash[adjustment.id()])
                        };
                    });

                    vm.adjustmentVMs(adjustmentMapVMs)

                    return vm.salary(salary);
                });

            Q.all([
                appointmentTypes,
                facultyTypes,
                meritAdjustmentTypes,
                rankTypes,
                salary,
                adjustmentTypes,
                statusTypes
            ]).fail(self.handleError);

            return true;
        }

        function saveChanges() {
            var self = this;

            applySelectionsToSalaryAdjustmentMaps();

            if (!unitofwork.hasChanges()) {
                return logger.log('No changes were detected.', null, null, true);
            }

            unitofwork.commit()
                .then(function () {
                    return logger.logSuccess('Save successful', null, null, true);
                })
                .fail(self.handleError);

            return true;
        }

        function createSalaryAdjustmentHash(salary) {
            var adjustmentHash = {};

            $.each(salary.specialSalaryAdjustments(), function (index, map) {
                adjustmentHash[map.specialAdjustmentTypeId()] = map;
            });

            return adjustmentHash;
        }

        function applySelectionsToSalaryAdjustmentMaps() {
            var salary = vm.salary(),

                mapVMs = vm.adjustmentVMs(),

                adjustmentHash = createSalaryAdjustmentHash(salary);

            $.each(mapVMs, function (index, mapVM) {
                var map = adjustmentHash[mapVM.adjustment.id()];

                if (mapVM.isSelected()) {
                    // User selected this adjustment.
                    if (!map) {
                        // No existing map, so create one.
                        map = unitofwork.specialSalaryAdjustments.create({
                            salaryId: salary.id(),
                            specialAdjustmentTypeId: mapVM.adjustment.id()
                        });
                    }
                } else {
                    // User unselected this adjustment.
                    if (map) {
                        // If map exists, delete it.
                        map.entityAspect.setDeleted();
                    }
                }
            });
        }
    });