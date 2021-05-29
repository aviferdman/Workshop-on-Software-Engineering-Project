import FormFieldInfo from "./formFieldInfo";
import FormFields from "./formFields";
import FormFieldCustomValidation from "./formFieldCustomValidation";

export default class CreditCardFields extends FormFields {
    constructor(fields) {
        super(fields || {
            number: new FormFieldInfo(''),
            year: new FormFieldInfo(''),
            month: new FormFieldInfo(''),
            cvv: new FormFieldInfo(''),
            holderName: new FormFieldInfo('forsen'),
            holderId: new FormFieldInfo('987654321'),
        });

        this.fields.number = new FormFieldCustomValidation('11112222333344445555', this.validate_number.bind(this));
        this.fields.year = new FormFieldCustomValidation('25', this.validate_year.bind(this));
        this.fields.month = new FormFieldCustomValidation('12', this.validate_month.bind(this));
        this.fields.cvv = new FormFieldCustomValidation('123', this.validate_cvv.bind(this));
    }

    validate_Ndigits(field, n) {
        if (!field.validate_required()) {
            return false;
        }
        else if (!new RegExp('^\\d{' + n + '}$').test(field.value)) {
            field.setError('Format: 2 digits');
            return false;
        }
        else {
            return true;
        }
    }

    validate_year() {
        let field = this.getField('year');
        if (!this.validate_Ndigits(field, 2)) {
            return false;
        }

        field.clearError();
        return true;
    }

    validate_month() {
        let field = this.getField('month');
        if (!this.validate_Ndigits(field, 2)) {
            return false;
        }

        let n = parseInt(field.value);
        if (!(1 <= n && n <= 12)) {
            field.setError('Number between 1-12');
            return false;
        }

        field.clearError();
        return true;
    }

    validate_cvv() {
        let field = this.getField('cvv');
        if (!this.validate_Ndigits(field, 3)) {
            return false;
        }

        field.clearError();
        return true;
    }

    validate_number() {
        return this.getField('number').validate_digits('Invalid format - digits only');
    }

    validate() {
        this.getField('number').validate();
        this.getField('year').validate();
        this.getField('month').validate();
        this.getField('cvv').validate();
        this.getField('holderName').validate();
        this.getField('holderId').validate();
        return !this.containsError();
    }
}