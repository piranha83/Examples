import { Component, OnInit } from '@angular/core';
import { EmploeeModel } from '../emploee.model';
import { DataService } from '../../Services/data.service';
import { FormlyFieldConfig } from '@ngx-formly/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-emploee-list',
  templateUrl: './emploee-list.component.html',
  styles: [``]
})
export class EmploeeListComponent implements OnInit {
    //public schema: string[] = ['id', 'fullName', 'salary', 'actions']
    public schema: any[] = [];
    public model: EmploeeModel[] = []

    constructor(public dataService: DataService) {}

    async ngOnInit(): Promise<void> {
      console.log('ListComponent');
      let [schema, model] = await Promise.all([
        this.dataService.GetDepartmentAvgSalarySchema(),
        this.dataService.GetDepartmentAvgSalary()
      ]);
      this.schema = schema.map(m=>m.key);
      this.model = model;      
    }

    async remove(id:number) {
      if(confirm('Удалить сотрудника?')) {
        let result = await this.dataService.DeleteEmploee(id).catch(ex => {
          console.log(ex);
          alert('Не удалось удалить');
        });
        if(result)
          this.model = await this.dataService.GetDepartmentAvgSalary();
      }
    }
}
