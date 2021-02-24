import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormlyFieldConfig } from '@ngx-formly/core';
import { EmploeeModel } from '../emploee/emploee.model';

@Injectable({ providedIn: 'root' })
export class DataService {
    constructor(private http: HttpClient) { console.log('DataService'); }

    public async GetEmploees(): Promise<EmploeeModel[]> {
        return await this.http.get<EmploeeModel[]>(`api/Employee`).toPromise()      
    }       

    public async GetEmploee(id: number): Promise<EmploeeModel> {
      return id
          ? await this.http.get<EmploeeModel>(`api/Employee/${id}`).toPromise()
          : await Promise.resolve(new EmploeeModel);
    }

    public async GetDepartment(): Promise<any[]> {
        return await this.http.get<any[]>(`api/Department`).toPromise()
    }

    public async SaveEmploee(data: EmploeeModel): Promise<any> {
        return data?.id
          ? await this.http.put(`api/Employee/${data.id}`, data).toPromise()     
          : await this.http.post(`api/Employee`, data).toPromise()
    }

    public async DeleteEmploee(id:number): Promise<any> {
        return await this.http.delete<any[]>(`api/Employee/${id}`).toPromise()
    }

    public async GetEmploeeSchema(): Promise<FormlyFieldConfig[]> {
        return await Promise.resolve([
          {
            key: 'Id',
            type: 'input',
            templateOptions: {
                required: true,
            },
            hideExpression: 'true',            
          },
          {
              key: 'firstName',
              type: 'input',
              templateOptions: {
                  label: 'Имя',
                  maxlength: 10,
                  required: true,
              },              
          },
          {
              key: 'lastName',
              type: 'input',
              templateOptions: {
                  type: 'text',
                  label: 'Фамилия',
                  required: false,
              },              
          },
          {
              key: 'salary',
              type: 'input',
              defaultValue: 0.00,
              templateOptions: {
                  type: 'number',
                  label: 'Зарплата',
                  required: true,
                  min: 0
              },
             
          },
          {
            key: 'departmentId',
            type: 'select',
            templateOptions: {
              label: 'Отдел',
              options: [] = await this.GetDepartment(),
              valueProp: 'id',
              labelProp: 'name'
            },             
        }              
      ]);
    }

    public async GetDepartmentAvgSalary(): Promise<any[]> {
        return await this.http.get<EmploeeModel[]>(`api/Employee/Department/AvgSalarySummary`).toPromise()      
    }

    public async GetDepartmentAvgSalarySchema(): Promise<FormlyFieldConfig[]> {
        return await Promise.resolve([
            {
                key: 'departmentName',
            },
            {
                key: 'averageSalary',
            },
            {
                key: 'employeesCount',
            },
            
        ]);
    }

    public async GetLoginSchema(): Promise<FormlyFieldConfig[]> {
        return await Promise.resolve([          
          {
              key: 'login',
              type: 'input',
              templateOptions: {
                  type: 'text',
                  label: 'Логин',
                  maxlength: 10,
                  required: true,
              },              
          },
          {
              key: 'password',
              type: 'input',
              templateOptions: {
                  type: 'text',
                  label: 'Пароль',
                  required: true,
              },              
          }                 
      ]);
    }
}
