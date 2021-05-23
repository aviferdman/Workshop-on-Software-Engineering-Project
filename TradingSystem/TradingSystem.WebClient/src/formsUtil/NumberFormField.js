import FormFieldInfo from "./formFieldInfo";

export default class NumberFormField extends FormFieldInfo {
    trySetValueFromEvent(e) {
        if (e.target.value === '') {
            this.value = '';
            return true;
        }

        let number = e.target.valueAsNumber;
        if (isNaN(number)) {
            return false;
        }

        this.value = number;
        return true;
    }
}