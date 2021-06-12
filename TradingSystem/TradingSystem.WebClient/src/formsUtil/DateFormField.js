import FormFieldInfo from "./formFieldInfo";

export default class DateFormField extends FormFieldInfo {
    constructor(initValue) {
        super(initValue === undefined || initValue === '' ? null : initValue);
    }

    trySetValueFromEvent(e) {
        let err = this.trySetValueFromEventCore(e);
        if (err) {
            e.target.value = this.inputValue;
        }
        return err;
    }

    trySetValueFromEventCore(e) {
        let value = e.target.value;
        if (value === '') {
            this.value = null;
            this.inputValue = '';
            return true;
        }

        if (e.target.valueAsDate == null) {
            this.value = null;
            this.inputValue = '';
            return true;
        }

        this.value = e.target.valueAsDate;
        this.inputValue = value;
        return true;
    }
}