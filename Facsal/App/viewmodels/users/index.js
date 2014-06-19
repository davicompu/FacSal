define(['services/unitofwork', 'services/errorhandler',
    'services/logger'],
    function (uow, errorhandler, logger) {

        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            selectedDepartmentId: ko.observable(),
            units: ko.observableArray(),
            users: ko.observableArray(),
        };

        errorhandler.includeIn(vm);

        vm.selectedDepartmentId.subscribe(function (newValue) {
            if (newValue != 'Choose...') {
                var users = unitofwork.usersByDepartment(newValue)
                    .then(function (response) {
                        $.each(response, function (index, value) {
                            response[index].formattedCreatedDate = ko.computed(function () {
                                return moment(response[index].createdDate).format('MM/DD/YYYY');
                            });
                        });
                        vm.users(response);
                    })
                    .fail(function (response) {
                        logger.logError(response.statusText, null, null, true);
                    });

                Q.all([users]).fail(self.handleError);
            }


            return true;
        });

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var self = this;

            var units = unitofwork.units.all()
                .then(function (response) {
                    vm.units(response);
                });

            Q.all([units]).fail(self.handleError);

            return true;
        }
    });