define(['services/appsecurity'],
    function (appsecurity) {
        var foreignKeyInvalidValue = 0;

        var self = {
            extendMetadata: extendMetadata
        };

        return self;

        function extendMetadata(metadataStore) {
            extendAdjustmentType(metadataStore);
            extendPaymentType(metadataStore);
            extendDepartment(metadataStore);
            extendFacultyType(metadataStore);
            extendPerson(metadataStore);
            extendRankType(metadataStore);
            extendSalary(metadataStore);
            extendUnit(metadataStore);
        }

        function extendAdjustmentType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('AdjustmentType', null, initializer);
        }

        function extendPaymentType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('PaymentType', null, initializer);
        }

        function extendDepartment(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Department', null, initializer);
        }

        function extendFacultyType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('FacultyType', null, initializer);
        }

        function extendPerson(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Person', null, initializer);
        }

        function extendRankType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('RankType', null, initializer);
        }

        function extendSalary(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);

                entity.percentIncrease = ko.computed(function () {
                    return (entity.newTotalAmount() / entity.totalAmount() - 1 * 100).formatNumber(1);
                });

                entity.meritPercentIncrease = ko.computed(function () {
                    return (entity.meritIncrease() / entity.totalAmount() - 1 * 100).formatNumber(1);
                });

                entity.specialPercentIncrease = ko.computed(function () {
                    return (entity.specialIncrease() / entity.totalAmount() - 1 * 100).formatNumber(1);
                });

                entity.eminentPercentIncrease = ko.computed(function () {
                    return (entity.eminentIncrease() / entity.totalAmount() - 1 * 100).formatNumber(1);
                });
            };

            metadataStore.registerEntityTypeCtor('Salary', null, initializer);
        }

        function extendUnit(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Unit', null, initializer);
        }

        //#region Internal Methods
        /**
         * Helper function for ensure the type of an entity is the requested
         * param {object} obj - The entity
         * param {string} entityTypeName - The type name
         */
        function ensureEntityType(obj, entityTypeName) {
            if (!obj.entityType || obj.entityType.shortName !== entityTypeName) {
                throw new Error('Object must be an entity of type ' + entityTypeName);
            }
        }

        /**
         * Add Knockout.Validation rules from the entities metadata
         */
        function addValidationRules(entity) {
            var entityType = entity.entityType,
                i,
                property,
                propertyName,
                propertyObject,
                validators,
                u,
                validator,
                nValidator;

            if (entityType) {
                for (i = 0; i < entityType.dataProperties.length; i += 1) {
                    property = entityType.dataProperties[i];
                    propertyName = property.name;
                    propertyObject = entity[propertyName];
                    validators = [];

                    for (u = 0; u < property.validators.length; u += 1) {
                        validator = property.validators[u];
                        nValidator = {
                            propertyName: propertyName,
                            validator: function (val) {
                                var error = this.innerValidator.validate(val, { displayName: this.propertyName });
                                this.message = error ? error.errorMessage : "";
                                return error === null;
                            },
                            message: "",
                            innerValidator: validator
                        };
                        validators.push(nValidator);
                    }
                    propertyObject.extend({
                        validation: validators
                    });
                }

                for (i = 0; i < entityType.foreignKeyProperties.length; i += 1) {
                    property = entityType.foreignKeyProperties[i];
                    propertyName = property.name;
                    propertyObject = entity[propertyName];

                    validators = [];
                    for (u = 0; u < property.validators.length; u += 1) {
                        validator = property.validators[u];
                        nValidator = {
                            propertyName: propertyName,
                            validator: function (val) {
                                var error = this.innerValidator.validate(val, { displayName: this.propertyName });
                                this.message = error ? error.errorMessage : "";
                                return error === null;
                            },
                            message: "",
                            innerValidator: validator
                        };
                        validators.push(nValidator);
                    }
                    propertyObject.extend({
                        validation: validators
                    });
                    if (!property.isNullable) {
                        // Business Rule: 0 is not allowed for required foreign keys.
                        propertyObject.extend({ notEqual: foreignKeyInvalidValue });
                    }
                }
            }
        }

        /**
         * Extend the entity with a has errors property
         * param {object} entity - The entity
         */
        function addHasValidationErrorsProperty(entity) {

            var prop = ko.observable(false);

            var onChange = function () {
                var hasError = entity.entityAspect.getValidationErrors().length > 0;
                if (prop() === hasError) {
                    // collection changed even though entity net error state is unchanged
                    prop.valueHasMutated(); // force notification
                } else {
                    prop(hasError); // change the value and notify
                }
            };

            onChange();             // check now ...
            entity.entityAspect // ... and when errors collection changes
                .validationErrorsChanged.subscribe(onChange);

            // observable property is wired up; now add it to the entity
            entity.hasValidationErrors = prop;
        }
        //#endregion
    });