/** 
	* @module UnitOfWork containing all repositories
	* @requires app
	* @requires entitymanagerprovider
	* @requires repository 
*/

define(['services/entitymanagerprovider', 'services/repository', 'durandal/app', 'services/config'],
	function (entityManagerProvider, repository, app, routeconfig) {

	    var refs = {};

	    /**
		* UnitOfWork ctor
		* @constructor
		*/
	    var UnitOfWork = (function () {

	        var unitofwork = function () {
	            var provider = entityManagerProvider.create();

	            /**
				* Has the current UnitOfWork changed?
				* @method
				* @return {bool}
				*/
	            this.hasChanges = function () {
	                return provider.manager().hasChanges();
	            };

	            /**
				* Commit changeset
				* @method
				* @return {promise}
				*/
	            this.commit = function () {
	                var saveOptions = new breeze.SaveOptions({ resourceName: routeconfig.saveChangesUrl });

	                return provider.manager().saveChanges(null, saveOptions)
						.then(function (saveResult) {
						    app.trigger('saved', saveResult.entities);
						});
	            };

	            /**
				* Rollback changes
				* @method
				*/
	            this.rollback = function () {
	                provider.manager().rejectChanges();
	            };

	            // Repositories
	            this.adjustmenttype = repository.create(provider, 'AdjustmentType',
                    routeconfig.adjustmentTypeUrl, breeze.FetchStrategy.FromLocalCache);
	            this.appointmenttype = repository.create(provider, 'AppointmentType',
                    routeconfig.appointmentTypeUrl, breeze.FetchStrategy.FromLocalCache);
	            this.department = repository.create(provider, 'Department',
                    routeconfig.departmentUrl, breeze.FetchStrategy.FromLocalCache);
	            this.facultyType = repository.create(provider, 'FacultyType',
                    routeconfig.facultyTypeUrl, breeze.FetchStrategy.FromLocalCache);
	            this.person = repository.create(provider, 'Person', routeconfig.personUrl);
	            this.rankType = repository.create(provider, 'RankType',
                    routeconfig.rankTypeUrl, breeze.FetchStrategy.FromLocalCache);
	            this.salary = repository.create(provider, 'Salary', routeconfig.salaryUrl);

	        };

	        return unitofwork;
	    })();

	    var SmartReference = (function () {

	        var ctor = function () {
	            var value = null;

	            this.referenceCount = 0;

	            this.value = function () {
	                if (value === null) {
	                    value = new UnitOfWork();
	                }

	                this.referenceCount++;
	                return value;
	            };

	            this.clear = function () {
	                value = null;
	                this.referenceCount = 0;

	                clean();
	            };
	        };

	        ctor.prototype.release = function () {
	            this.referenceCount--;
	            if (this.referenceCount === 0) {
	                this.clear();
	            }
	        };

	        return ctor;
	    })();

	    return {
	        create: create,
	        get: get
	    };

	    /**
		 * Get a new UnitOfWork instance
		 * @method
		 * @return {UnitOfWork}
		*/
	    function create() {
	        return new UnitOfWork();
	    }

	    /**
		 * Get a new UnitOfWork based on the provided key
		 * @method
		 * @param {int/string} key - Key used in the reference store
		 * @return {promise}
		*/
	    function get(key) {
	        if (!refs[key]) {
	            refs[key] = new SmartReference();
	        }

	        return refs[key];
	    }

	    /**
		 * Delete references
		 * @method         
		*/
	    function clean() {
	        for (var key in refs) {
	            if (refs[key].referenceCount === 0) {
	                delete refs[key];
	            }
	        }
	    }
	});
