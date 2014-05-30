define(['services/repository'],
    function (repository) {

        var EmploymentRepository = (function () {
            var repo = function (entityManagerProvider, entityTypeName, resourceName, fetchStrategy) {
                repository.getCtor.call(this, entityManagerProvider, entityTypeName, resourceName, fetchStrategy);

                this.find = function (predicate) {
                    var query = breeze.EntityQuery
                        .from(resourceName)
                        .where(predicate)
                        .expand('Person.Salaries');

                    return executeQuery(query);
                };

                function executeQuery(query) {
                    return entityManagerProvider.manager()
                        .executeQuery(query.using(fetchStrategy || breeze.FetchStrategy.FromServer))
                        .then(function (data) {
                            return data.results;
                        });
                }
            };

            repo.prototype = repository.create();
            return repo;
        })();

        return {
            create: create
        };

        function create(entityManagerProvider, entityTypeName, resourceName, fetchStrategy) {
            return new EmploymentRepository(entityManagerProvider, entityTypeName, resourceName, fetchStrategy);
        }
    });