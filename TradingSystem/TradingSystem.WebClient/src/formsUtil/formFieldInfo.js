export default class FormFieldInfo {
    constructor(initValue) {
        this.value = initValue;
        this.errorMessage = null;
        this.isError = false;
    }

    setError(msg) {
        this.isError = true;
        this.errorMessage = msg;
    }

    clearError() {
        this.isError = false;
        this.errorMessage = '';
    }

    validate() {
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