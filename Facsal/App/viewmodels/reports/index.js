define(['services/unitofwork', 'services/errorhandler',
    'services/config', 'plugins/router', 'services/logger'],
    function (uow, errorhandler, config, router, logger) {
        var unitofwork = uow.create();

        var vm = {
            activate: activate,
            attached: attached,

            audienceTypes: ko.observableArray(),
            reportTypes: ko.observableArray(),
            selectedAudienceType: ko.observable(),
            selectedDepartmentId: ko.observable(),
            selectedReportType: ko.observable(),
            selectedUnitId: ko.observable(),
            units: ko.observableArray(),

            generateReport: generateReport,
        };

        errorhandler.includeIn(vm);

        vm.selectedAudienceType.subscribe(function () {
            vm.selectedDepartmentId(undefined);
            vm.selectedUnitId(undefined);

            return true;
        });

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached() {
            var self = this,

                audienceTypes = [
                    'Department',
                    'Unit'
                ],

                reportTypes = [
                    { route: 'meeting', text: 'Meeting' },
                    { route: 'base-salary-adjustment', text: 'Faculty with base salary adjustments' },
                    { route: 'multiple-employments', text: 'Faculty with multiple departments' },
                    { route: 'salaries-by-faculty-type', text: 'Salaries by faculty type' },
                    { route: 'unreviewed', text: 'Unreviewed faculty' }
                ],

                units = unitofwork.units.all()
                    .then(function (response) {
                        vm.units(response);
                    });

            Q.all([units]).fail(self.handleError);

            vm.audienceTypes(audienceTypes);
            
            vm.reportTypes(reportTypes);

            return true;
        }

        function generateReport() {
            var route;

            switch (vm.selectedAudienceType()) {
                case 'Department':
                    if (vm.selectedDepartmentId() && vm.selectedDepartmentId() !== 'Choose...') {
                        route = 'reports/' +
                        vm.selectedReportType() +
                        '/' + vm.selectedDepartmentId();

                        router.navigate(route);
                    } else {
                        logger.logError('Choose a department to continue.', null, null, true);
                    }

                    break;
                case 'Unit':
                    if (vm.selectedUnitId()) {
                        route = 'reports/' +
                            vm.selectedReportType() +
                            '/' + vm.selectedUnitId();

                        router.navigate(route);
                    } else {
                        logger.logError('Choose a unit to continue.', null, null, true);
                    }

                    break;
            }
        }
    });