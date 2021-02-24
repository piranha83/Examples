import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormlyFieldConfig } from '@ngx-formly/core';
import { JwtModel } from '.';
import { StorageService } from '../services';

@Injectable({ providedIn: 'root' })
export class JwtService {
    constructor(
        private http: HttpClient,
        private storageService: StorageService) { 
            console.log('LoginService'); 
    }

    public async GetSchema(): Promise<FormlyFieldConfig[]> {
        return await Promise.resolve([          
          {
              key: 'login',
              type: 'input',
              templateOptions: {
                  type: 'text',
                  label: 'Логин',
                  minLength: 3,
                  required: true,
              },              
          },
          {
              key: 'password',
              type: 'input',
              templateOptions: {
                  type: 'password',
                  label: 'Пароль',
                  required: true,
                  minLength: 5,
              },              
          }                 
      ]);
    }

    public async Login(data: { login: string, password: string }): Promise<boolean> {
        const res = await this.http.post(`api/jwt/Authenticate`, data).toPromise<any>();
        if(res && res.token && res.userName) {
            this.storageService.store('jwt', res);
            return true;
        }
        return false;
    }

    public Logout(): void {
        this.storageService.store('jwt', null);
    }

    public get Identity(): JwtModel
    {
        return <JwtModel>this.storageService.retrieve('jwt');        
    }

    public get IsAuth(): boolean
    {
        const res = this.storageService.retrieve('jwt');
        return res && res.token && res.token.length > 1;        
    }
}
