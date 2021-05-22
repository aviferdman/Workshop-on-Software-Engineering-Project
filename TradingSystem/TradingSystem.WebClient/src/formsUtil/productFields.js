import FormFields from "./formFields";
import NumberFormField from "./NumberFormField";

export default class ProductFields extends FormFields {
    constructor(fields) {
        super(fields || {
            name: "",
            quantity: new NumberFormField(''),
            price: new NumberFormField(''),
            category: "",
            weight: new NumberFormField(''),
        });
    }
}