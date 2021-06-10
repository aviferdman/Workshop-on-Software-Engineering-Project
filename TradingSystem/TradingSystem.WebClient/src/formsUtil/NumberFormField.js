import FormFieldInfo from "./formFieldInfo";

export default class NumberFormField extends FormFieldInfo {
    trySetValueFromEvent(e) {
        let value = e.target.value;
        if (value === '') {
            this.value = '';
            this.inputValue = value;
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