import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormService } from '../services';
import { JwtService } from './jwt.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  providers: [ FormService ],
  styles: ['']
})
export class LoginComponent {

  constructor(
    public formService: FormService,
    private dataService: JwtService,
    private router: Router) {      
  }

  public async ngOnInit(): Promise<void> {
    console.log('EmploeeFormComponent');
    this.formService.schema = await this.dataService.GetSchema();
    this.formService.model = { login: '', password: '' };
    this.dataService.Logout();
  }

  public async onSubmit(model: any): Promise<void> {
    if (this.formService.form.valid != true) return;
    if(await this.dataService.Login(model).catch(ex => this.error()))
    {
      this.router.navigate(['emploee']);
    } else {
      this.error();
    }
  }

  error(): void {
    this.formService.setErrors({ password: ['Неверный пароль, попробуйте ещё раз']});
  }
}
