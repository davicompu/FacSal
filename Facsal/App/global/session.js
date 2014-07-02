define([],
    function () {

        var session = {
            userName: ko.observable(undefined),
            isLoggedIn: ko.observable(false),
            userRoles: ko.observableArray(),
            userIsInRole: userIsInRole,
            setUser: setUser,
            clearUser: clearUser,
        };

        return session;

        function setUser(user) {
            if (user) {

                session.userName(user.userName);

                var roles = user.userRoles.split(",");

                $.each(roles, function (i, v) {
                    session.userRoles.push(v);
                });

                session.isLoggedIn(true);
            }
        }

        function clearUser() {
            session.userName('');
            session.userRoles.removeAll();
            session.isLoggedIn(false);
        }

        function userIsInRole(requiredRole) {
            if (requiredRole === undefined) {
                return true;
            } else if (session.userRoles() === undefined) {
                return false;
            } else {
                if ($.isArray(requiredRole)) {
                    if (requiredRole.length === 0) {
                        return true;
                    } else {
                        return $.arrayIntersect(
                            session.userRoles(), requiredRole).length > 0;
                    }
                } else {
                    return $.inArray(requiredRole, session.userRoles()) > -1;
                }
            }
        }
    });