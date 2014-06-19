define(['services/unitofwork', 'services/errorhandler',
    'services/config'],
    function (uow, errorhandler, config) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            deactivate: deactivate,

            departmentData: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        return vm;

        function activate(unitId, departmentId) {
            var self = this;

            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            if (departmentId) {
                var department = unitofwork.departments.withId(departmentId)
                    .then(function (response) {
                        getData([response.entity]);
                    });

                Q.all([
                    department
                ]).fail(self.handleError);
            } else {
                var predicate = breeze.Predicate.create(
                    'toLower(unitId)', '==', unitId.toLowerCase()),

                    departments = unitofwork.departments.find(predicate)
                        .then(function (response) {
                            getData(response);
                        });

                Q.all([
                    departments
                ]).fail(self.handleError);
            }

            return true;
        }

        function deactivate() {
            vm.departmentData([]);

            return true;
        }

        function getData(departments) {
            var self = this;

            return $.each(departments, function (index, department) {
                var salaries = unitofwork.salariesByFacultyType(department.id())
                    .then(function (response) {
                        $.each(response, function (index, value) {
                            response[index].formattedStartingSalaries =
                                ko.observable(response[index].startingSalaries).extend({ currency: [0] });
                            response[index].formattedMeritIncreases =
                                ko.observable(response[index].meritIncreases).extend({ currency: [0] });
                            response[index].formattedSpecialIncreases =
                                ko.observable(response[index].specialIncreases).extend({ currency: [0] });
                            response[index].formattedEminentIncreases =
                                ko.observable(response[index].eminentIncreases).extend({ currency: [0] });
                            response[index].formattedNewSalaries =
                                ko.observable(response[index].newSalaries).extend({ currency: [0] });
                        });

                        return vm.departmentData.push({
                            department: department,
                            data: response
                        });
                    });

                Q.all([
                    salaries
                ]).fail(self.handleError);
            });
        }
    });