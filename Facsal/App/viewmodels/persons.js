define(['services/dataservice'],
    function (dataservice) {
        var persons = ko.observableArray();

        var initialized = false;

        var vm = {
            activate: activate,
            persons: persons,
            title: 'Browse Persons',
            refresh: refresh,
        };

        return vm;

        function activate() {
            if (initialized) { return true; }
            initialized = true;
            return refresh();
        }

        function refresh() {
            return dataservice.getPersons(persons);
        }
    });