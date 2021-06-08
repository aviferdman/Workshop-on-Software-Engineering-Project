export default class FormFieldInfo {
    constructor(initValue) {
        this.value = initValue === undefined ? '' : initValue;
        this.inputValue = this.value;
        this.errorMessage = null;
        this.isError = false;
        this.isValidationOn = true;
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
        this.inputValue = this.value;
        return true;
    }

    setValidationOn() {
        this.isValidationOn = true;
    }

    setValidationOff() {
        this.isValidationOn = false;
    }

    validate() {
        if (!this.isValidationOn) {
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