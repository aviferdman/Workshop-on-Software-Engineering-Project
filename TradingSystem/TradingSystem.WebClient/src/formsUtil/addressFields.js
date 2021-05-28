import FormFieldInfo from "./formFieldInfo";
import FormFields from "./formFields";
import FormFieldCustomValidation from "./formFieldCustomValidation";

export default class AddressFields extends FormFields {
    constructor(fields) {
        super(fields || {
            state: new FormFieldInfo('Svenden'),
            city: new FormFieldInfo('Malmo'),
            street: new FormFieldInfo('NaM'),
            apartmentNumber: new FormFieldInfo('12'),
            zipCode: new FormFieldInfo(''),
        });
        // this.fields.apartmentNumber = new FormFieldCustomValidation('', this.validate_digits.bind(this, 'apartmentNumber'));
        this.fields.zipCode = new FormFieldCustomValidation('123456', this.validate_digits.bind(this, 'zipCode'));
    }

    validate_digits(key) {
        return this.getField(key).validate_digits('Invalid format - not only digits');
    }

    validate() {
        this.getField('state').validate();
        this.getField('city').validate();
        this.getField('street').validate();
        this.getField('apartmentNumber').validate();
        this.getField('zipCode').validate();
        return !this.containsError();
    }
}