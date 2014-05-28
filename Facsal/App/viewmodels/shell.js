﻿define(['plugins/router', 'services/security', 'services/errorhandler', 'services/entitymanagerprovider', 'model/modelBuilder'],
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
                { route: ['', 'salaries', 'salaries/index'], moduleId: 'salaries/index', title: 'Salaries', nav: true, hash: '#salaries/index' }
            ])
            .buildNavigationModel()
            .mapUnknownRoutes("notfound", "notfound")
            .activate({ pushState: true });
        }
    });