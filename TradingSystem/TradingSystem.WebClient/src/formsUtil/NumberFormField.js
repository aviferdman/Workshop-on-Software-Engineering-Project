import FormFieldInfo from "./formFieldInfo";

export default class NumberFormField extends FormFieldInfo {
    trySetValueFromEvent(e) {
        let number = e.target.valueAsNumber;
        if (isNaN(number)) {
            return false;
        }

        this.value = number;
        return true;
    }
}