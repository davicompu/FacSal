define(['services/unitofwork', 'services/errorhandler',
    'services/config', 'plugins/router'],
    function (uow, errorhandler, config, router) {
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

        vm.selectedAudienceType.subscribe(function (newValue) {
            vm.selectedDepartmentId(undefined);
            vm.selectedUnitId(undefined);
        });

        return vm;

        function activate() {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            return true;
        }

        function attached(view) {
            var self = this,

                audienceTypes = [
                    'Department',
                    'Unit'
                ],

                reportTypes = [
                    'Meeting',
                    'Faculty with base salary adjustments',
                    'Faculty with multiple departments',
                    'Salaries by faculty type',
                    'Unreviewed faculty'
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
            switch (vm.selectedAudienceType()) {
                case 'Department':
                    router.navigate('reports/')
                    break;
            }
        }
    });