export default class FormFields {
    constructor(fields) {
        this.fields = fields;
    }

    validate() {
        throw new Error('abstract method');
    }

    containsError() {
        let keys = Object.keys(this.fields);
        for (let i = 0; i < keys.length; i++) {
            let key = keys[i];
            if (this.getField(key).isError) {
                return true;
            }
        }

        return false;
    }

    getField(name) {
        return this.fields[name];
    }

    setValue(name, value) {
        this.getField(name).value = value;
        return this;
    }

    getValue(name) {
        return this.getField(name).value;
    }

    setError(name, msg) {
        this.getField(name).setError(msg);
    }

    clearError(name) {
        this.getField(name).clearError();
    }

    isError(name) {
        return this.getField(name).isError;
    }

    getErrorMessageOf(name) {
        return this.getField(name).errorMessage;
    }
}