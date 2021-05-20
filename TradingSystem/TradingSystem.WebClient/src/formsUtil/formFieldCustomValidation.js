import FormFieldInfo from "./formFieldInfo";

export default class FormFieldCustomValidation extends FormFieldInfo {
    constructor(initValue, validationLogic) {
        super(initValue);
        this.validationLogic = validationLogic;
    }

    validate() {
        if (!super.validate()) {
            return false;
        }
        return this.validationLogic(this.value, this);
    }
}