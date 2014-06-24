define(['services/utils', 'services/config'],
    function (utils, config) {
        var foreignKeyInvalidValue = 0;

        var self = {
            extendMetadata: extendMetadata
        };

        return self;

        function extendMetadata(metadataStore) {
            extendAppointmentType(metadataStore);
            extendBaseSalaryAdjustment(metadataStore);
            extendDepartment(metadataStore);
            extendEmployment(metadataStore);
            extendFacultyType(metadataStore);
            extendMeritAdjustmentType(metadataStore);
            extendPerson(metadataStore);
            extendRankType(metadataStore);
            extendSalary(metadataStore);
            extendSpecialAdjustmentType(metadataStore);
            extendUnit(metadataStore);
            extendUser(metadataStore);
        }

        function extendAppointmentType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('AppointmentType', null, initializer);
        }

        function extendBaseSalaryAdjustment(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);

                entity.formattedCreatedDate = ko.computed(function () {
                    return moment(entity.createdDate()).format('MM/DD/YYYY');
                });

                entity.formattedStartingBaseAmount = ko.observable(entity.startingBaseAmount()).extend({ currency: [0] });

                entity.formattedNewBaseAmount = ko.observable(entity.newBaseAmount()).extend({ currency: [0] });
            };

            metadataStore.registerEntityTypeCtor('BaseSalaryAdjustment', null, initializer);
        }

        function extendDepartment(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Department', null, initializer);
        }

        function extendEmployment(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Employment', null, initializer);
        }

        function extendFacultyType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('FacultyType', null, initializer);
        }

        function extendMeritAdjustmentType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('MeritAdjustmentType', null, initializer);
        }

        function extendPerson(metadataStore) {
            var personCtor = function () {
                this.id = ko.observable(breeze.core.getUuid());
            }

            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Person', personCtor, initializer);
        }

        function extendRankType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('RankType', null, initializer);
        }

        function extendRole(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Role', null, initializer);
        }

        function extendSalary(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);

                entity.totalAmount = ko.computed(function () {
                    return entity.baseAmount() + entity.adminAmount() +
                        entity.eminentAmount() + entity.promotionAmount();
                });

                entity.newTotalAmount = ko.computed(function () {
                    return entity.totalAmount() +
                        entity.meritIncrease() +
                        entity.specialIncrease() +
                        entity.eminentIncrease();
                });

                entity.percentIncrease = ko.computed(function () {
                    return (entity.newTotalAmount() / entity.totalAmount() - 1);
                });

                entity.meritPercentIncrease = ko.computed(function () {
                    return ((entity.meritIncrease() / entity.totalAmount() - 1) * 100).formatNumber(1);
                });

                entity.specialPercentIncrease = ko.computed(function () {
                    return ((entity.specialIncrease() / entity.totalAmount() - 1) * 100).formatNumber(1);
                });

                entity.eminentPercentIncrease = ko.computed(function () {
                    return ((entity.eminentIncrease() / entity.totalAmount() - 1) * 100).formatNumber(1);
                });

                entity.formattedTotalAmount = ko.observable(entity.totalAmount()).extend({ currency: [0] });

                entity.formattedMeritIncrease = ko.observable(entity.meritIncrease()).extend({ currency: [0] });

                entity.formattedSpecialIncrease = ko.observable(entity.specialIncrease()).extend({ currency: [0] });

                entity.formattedEminentIncrease = ko.observable(entity.eminentIncrease()).extend({ currency: [0] });

                entity.formattedNewTotalAmount = ko.observable(entity.newTotalAmount()).extend({ currency: [0] });

                entity.formattedPercentIncrease = ko.observable(entity.percentIncrease()).extend({ percent: 1 });

                entity.meritAdjustmentNote
                .extend({
                    required: {
                        onlyIf: function () {
                            var increase = entity.meritIncrease() / entity.totalAmount();

                            return increase > config.highPercentIncreaseThreshold ||
                                increase < config.lowPercentIncreaseThreshold
                        },
                        message: 'This field is required.'
                    }
                });

                entity.specialAdjustmentNote
                    .extend({
                        required: {
                            onlyIf: function () {
                                return entity.specialIncrease() !== 0;
                            },
                            message: 'This field is required.'
                        }
                    });
            };

            metadataStore.registerEntityTypeCtor('Salary', null, initializer);
        }

        function extendSpecialAdjustmentType(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('SpecialAdjustmentType', null, initializer);
        }

        function extendUnit(metadataStore) {
            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);
            };

            metadataStore.registerEntityTypeCtor('Unit', null, initializer);
        }

        function extendUser(metadataStore) {
            var userCtor = function () {
                //this.id = ko.observable(breeze.core.getUuid());
            }

            var initializer = function (entity) {
                addValidationRules(entity);
                addHasValidationErrorsProperty(entity);

                entity.formattedCreatedDate = ko.computed(function () {
                    return moment(entity.createdDate()).format('MM/DD/YYYY');
                });
            };

            metadataStore.registerEntityTypeCtor('User', userCtor, initializer);
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