define(['logger', 'durandal/system'],
    function (logger, system) {
    var getPersons = function (personsObservable) {
        // Reset the observable.
        personsObservable([]);

        // Make the data call.
        return;

        // Handle the callback.
        function querySucceeded(data) {
            var persons = [];
            data.foreach(function (item) {
                var person = new model.PersonPartial(item);
                persons.push(person);
            });

            // Put persons into observable afterwards to avoid multiple notification events.
            personsObservable(persons);
            log('Retrieved persons from remote data source.',
                persons,
                true);
        }
    }

    var dataservice = {
        getPersons: getPersons,
    };

    return dataservice;

    //#region Internal methods.
    function log(msg, data, showToast) {
        logger.log(
            msg,
            data,
            system.getModuleId(dataservice),
            showToast);
    }

    function queryFailed(jqXHR, textStatus) {
        var msg = 'Error retrieving data. ' + textStatus;
        logger.log(msg,
            jqXHR,
            system.getModuleId(dataservice),
            true);
    }
    //#endregion
})