﻿ko.extenders.numeric = function(target, precision) {
    // Create a writeable computed observable to intercept writes to our observable.
    var result = ko.computed({
        read: target,  //always return the original observables value
        write: function(newValue) {
            var current = target(),
                roundingMultiplier = Math.pow(10, precision),
                newValueAsNum = isNaN(newValue) ? 0 : parseFloat(+newValue),
                valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;
 
            // Only write if value changed.
            if (valueToWrite !== current) {
                target(valueToWrite);
            } else {
                // If the rounded value is the same but a different value was written, force a notification for the current field.
                if (newValue !== current) {
                    target.notifySubscribers(valueToWrite);
                }
            }
        }
    }).extend({ notify: 'always' });
 
    // Initialize with current value to make sure it is rounded appropriately.
    result(target());
 
    // Return the new computed observable.
    return result;
};

ko.extenders.currency = function (target, configArray) {
    var precision = configArray[0],
        rawValueObservable = configArray[1] || ko.observable();
    // Create a writeable computed observable to intercept writes to our observable.
    var result = ko.computed({
    	// Always return the original observable's value.
        read: target,  
        write: function (newValue) {
            var current = target(),
                cleansedValue = parseFloat(newValue.toString().replace(/[^\d.-]/g, '')),
                newValueAsNum = isNaN(cleansedValue) ? 0 : parseFloat(+cleansedValue),
                isNegativeValue = newValueAsNum < 0 ? true : false,
                valueToFormat = parseFloat(newValue.toString().replace(/[^\.\d]/g, '')),
                formattedValue = valueToFormat.formatNumber(precision),
                valueToWrite = isNegativeValue ? '($' + formattedValue + ')' : '$' + formattedValue;

        	// Only write if value changed.
            if (valueToWrite !== current) {
                rawValueObservable(newValueAsNum);
                target(valueToWrite);
            } else {
            	// If the rounded value is the same but a different value was written, force a notification for the current field.
            	if (newValue !== current) {
                    target.notifySubscribers(valueToWrite);
                }
            }
        }
    }).extend({ notify: 'always' });

	// Initialize with current value to make sure it is rounded appropriately.
    result(target());

	// Return the new computed observable.
    return result;
};