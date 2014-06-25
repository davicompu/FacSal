define(['plugins/router', 'services/security', 'services/errorhandler',
    'services/entitymanagerprovider', 'model/modelBuilder', 'services/logger',
    'global/session', 'services/config'],
    function (router, appsecurity, errorhandler, entitymanagerprovider, modelBuilder,
        logger, session, config) {

        entitymanagerprovider.modelBuilder = modelBuilder.extendMetadata;

        var viewmodel = {
            activate: function () {
                var self = this;

                return entitymanagerprovider.prepare()
                    .then(init)
                    .fail(self.handlevalidationerrors);
            },
            attached: function (view) {
                $(view).foundation();
            },

            router: router,
            username: ko.observable(),

            logOut: logOut,
        };

        errorhandler.includeIn(viewmodel);

        return viewmodel;

        function logOut() {
            window.location.assign(config.logOutUrl);
        }

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
                // Nav.
                {
                    route: ['', 'salaries', 'salaries/index'], moduleId: 'salaries/index',
                    title: 'Salaries', nav: true, hash: '#salaries/index'
                },
                {
                    route: 'salaries/search', moduleId: 'salaries/search',
                    title: 'Search', nav: true, hash: '#salaries/search'
                },
                {
                    route: ['reports', 'reports/index'], moduleId: 'reports/index',
                    title: 'Reports', nav: true, hash: '#reports/index'
                },

                // Persons.
                {
                    route: 'persons/create', moduleId: 'persons/create',
                    title: 'New employee', nav: false, hash: '#persons/create'
                },

                // Reports.
                {
                    route: 'reports/base-salary-adjustment/:unitid(/:departmentid)',
                    moduleId: 'reports/base-salary-adjustment',
                    title: 'Base salary adjustment report', nav: false,
                    hash: '#reports/base-salary-adjustment'
                },
                {
                    route: 'reports/meeting/:unitid(/:departmentid)',
                    moduleId: 'reports/meeting',
                    title: 'Meeting report', nav: false, hash: '#reports/meeting'
                },
                {
                    route: 'reports/multiple-employments/:unitid(/:departmentid)',
                    moduleId: 'reports/multiple-employments',
                    title: 'Multiple departments report', nav: false,
                    hash: '#reports/multiple-employments'
                },
                {
                    route: 'reports/salaries-by-faculty-type/:unitid(/:departmentid)',
                    moduleId: 'reports/salaries-by-faculty-type',
                    title: 'Salaries by faculty type report', nav: false,
                    hash: '#reports/salaries-by-faculty-type'
                },
                {
                    route: 'reports/unreviewed/:unitid(/:departmentid)',
                    moduleId: 'reports/unreviewed',
                    title: 'Unreviewed faculty report', nav: false,
                    hash: '#reports/unreviewed'
                },

                // Salaries.
                {
                    route: 'salaries/edit/:salaryid', moduleId: 'salaries/edit',
                    title: 'Edit salary', nav: false, hash: '#salaries/edit/:salaryid'
                },

                // Users.
                {
                    route: ['users', 'users/index'], moduleId: 'users/index',
                    title: 'Users', nav: false, hash: '#users/index'
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

        function init() {
            return appsecurity.getUserInfo()
                .done(function (data) {
                    if (data.userName) {
                        session.setUser(data);
                        viewmodel.username(data.userName);
                        initializeRouting();
                    } else {
                        logger.log("Access denied. Navigation canceled.",
                            null,
                            'viewmodels/shell',
                            true,
                            "warning"
                        );

                        initializeRouting();
                    }
                })
                .fail(function () {
                    logger.log("Access denied. Navigation canceled.",
                        null,
                        'viewmodels/shell',
                        true,
                        "warning"
                    );

                    initializeRouting();
                });
        }
    });