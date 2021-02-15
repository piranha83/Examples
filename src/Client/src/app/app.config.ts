import { FormControl } from "@angular/forms";
import { ConfigOption, FormlyFieldConfig } from "@ngx-formly/core";

export const AppConfig: ConfigOption = {
    extras: {
      checkExpressionOn: 'modelChange',
    },
    validationMessages: [
        { 
            name: 'currency', 
            message: (err: any, field: FormlyFieldConfig): 
              string => `"${field.form?.controls?.value}" формат валюты не соответствует 000000,00`
        }
    ],
    types: [
        {
          name: 'currency',
          extends: 'input',
          defaultOptions: {
            validators: {
              currency: (control: FormControl): 
                  boolean => /^[0-9]+(\,[0-9]{1,2})?$/.test(control.value)
            }
          }
        }
    ]
};