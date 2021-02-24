import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormlyFieldConfig, FormlyFormOptions } from '@ngx-formly/core';
import { BehaviorSubject, Observable } from 'rxjs';

export class FormService {
  public form: FormGroup = new FormGroup({}, { updateOn: 'blur' });
  public model: any = {};
  public options: FormlyFormOptions = {
    formState: {
      errors$: new BehaviorSubject<any>(null),
    }
  };
  public schema: FormlyFieldConfig[] = []

  public get Errors(): Observable<any> {
    return this.options.formState.errors$.asObservable();
  }

  public setErrors(errors: any): void {
    this.options.formState.errors$.next(errors);
    Object.entries(errors).forEach(([key, value]: any) => {
      const field = this.fix(key);
      console.log(key, value, this.form, this.form.get(field));
      const error = (value instanceof Array && value.length > 0 ? value[0] : value);
      this.form.get(field)?.setErrors({ server: { message: error }});
    });
  }

  private fix(name:string): string {
    var temp = name?.replace(/\W+/, '');
    return temp.length > 1 ? temp.charAt(0).toLowerCase() + temp.substr(1) : temp;
  }
}