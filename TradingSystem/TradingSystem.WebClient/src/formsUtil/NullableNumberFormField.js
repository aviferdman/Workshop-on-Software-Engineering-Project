import FormFieldInfo from "./formFieldInfo";

export default class NullableNumberFormField extends FormFieldInfo {
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

        let number = e.target.valueAsNumber;
        if (isNaN(number)) {
            return false;
        }

        this.value = number;
        this.inputValue = value;
        return true;
    }
}