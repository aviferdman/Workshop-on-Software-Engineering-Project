import FormFieldInfo from "./formFieldInfo";

export default class CheckboxFormField extends FormFieldInfo {
    constructor(initValue) {
        super(initValue === undefined ? false : initValue);
    }

    trySetValueFromEvent(e) {
        let value = e.target.checked;
        this.setValue(value, value);
        return true;
    }
}