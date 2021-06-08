export default class FormFieldInfo {
    constructor(initValue) {
        this.value = initValue === undefined ? '' : initValue;
        this.errorMessage = null;
        this.isError = false;
        this.validationCounterPushCounter = 0;
    }

    setError(msg) {
        this.isError = true;
        this.errorMessage = msg;
    }

    clearError() {
        this.isError = false;
        this.errorMessage = '';
    }

    trySetValueFromEvent(e) {
        this.value = e.target.value;
        return true;
    }

    popValidationOn() {
        this.validationCounterPushCounter--;
    }

    pushValidationOff() {
        this.validationCounterPushCounter++;
    }

    validate() {
        if (this.validationCounterPushCounter > 0) {
            return true;
        }

        return this.validate_required();
    }

    validate_required() {
        if (this.value == null || this.value === '') {
            this.setError('Required');
        }
        else {
            this.clearError();
        }

        return !this.isError;
    }

    validate_digits(errMsg) {
        if (!this.validate_required()) {
            return false;
        }
        else if (!/^\d+$/.test(this.value)) {
            this.setError(errMsg);
        }
        else {
            this.clearError();
        }

        return !this.isError;
    }
}