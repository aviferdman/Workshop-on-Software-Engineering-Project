import FormFieldInfo from "./formFieldInfo";

export default class DateFormField extends FormFieldInfo {
    constructor(initValue) {
        super(initValue === undefined || initValue === '' ? null : initValue);
    }

    trySetValueFromEvent(e) {
        let value = e.target.value;
        if (value === '') {
            this.value = null;
            this.inputValue = '';
            return true;
        }

        if (e.target.valueAsDate == null) {
            return false;
        }

        this.value = e.target.valueAsDate;
        this.inputValue = value;
        return true;
    }
}