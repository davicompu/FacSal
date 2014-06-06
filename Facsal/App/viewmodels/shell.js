define(['plugins/router', 'services/security', 'services/errorhandler', 'services/entitymanagerprovider', 'model/modelBuilder'],
    function (router, appsecurity, errorhandler, entitymanagerprovider, modelBuilder) {

        entitymanagerprovider.modelBuilder = modelBuilder.extendMetadata;

        var viewmodel = {

            router: router,

            activate: function () {
                var self = this;

                return entitymanagerprovider.prepare()
                    .then(initializeRouting)
                    .fail(self.handlevalidationerrors);
            }
        };

        errorhandler.includeIn(viewmodel);

        return viewmodel;

        function initializeRouting() {
            //configure routing
            router.makeRelative({ moduleId: 'viewmodels' });

            router.guardRoute = function (routeInfo, params, instance) {
                if (typeof (params.config.requiredRoles) !== "undefined") {
                    var res = session.userIsInRole(params.config.requiredRoles);

                    if (!res) {
                        logger.log("Access denied. Navigation canceled.",
                            null,
                            'viewmodels/shell',
                            true,
                            "warning"
                        );
                    }

                    return res;
                } else {
                    return true;
                }
            };

            return router.map([
                // Persons.
                {
                    route: 'persons/create', moduleId: 'persons/create',
                    title: 'New employee', nav: false, hash: '#persons/create'
                },

                // Salaries.
                {
                    route: ['', 'salaries', 'salaries/index'], moduleId: 'salaries/index',
                    title: 'Salaries', nav: true, hash: '#salaries/index'
                },
                {
                    route: 'salaries/edit/:salaryid', moduleId: 'salaries/edit',
                    title: 'Edit salary', nav: false, hash: '#salaries/edit/:salaryid'
                },
                {
                    route: 'salaries/search', moduleId: 'salaries/search',
                    title: 'Search', nav: true, hash: '#salaries/search'
                },

                // Users.
                {
                    route: ['users', 'users/index'], moduleId: 'users/index',
                    title: 'Users', nav: true, hash: '#users/index'
                },
                {
                    route: 'users/edit/:userid', moduleId: 'users/edit',
                    title: 'Edit user', nav: false, hash: '#users/edit/:salaryid'
                },
                {
                    route: 'users/create', moduleId: 'users/create',
                    title: 'New user', nav: false, hash: '#users/create'
                },

                // Route not found.
                { route: 'not-found', moduleId: 'not-found', title: 'Not found', nav: false }
            ])
            .buildNavigationModel()
            .mapUnknownRoutes("not-found", "not-found")
            .activate({ pushState: true });
        }
    });