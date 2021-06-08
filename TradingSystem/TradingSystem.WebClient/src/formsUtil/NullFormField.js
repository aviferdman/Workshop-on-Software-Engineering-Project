import FormFieldInfo from "./formFieldInfo";

export default class NullFormField extends FormFieldInfo {
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

        return super.trySetValueFromEvent(e);
    }
}