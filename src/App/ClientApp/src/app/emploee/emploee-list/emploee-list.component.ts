import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { JwtService } from 'src/app/login';
import { DataService } from 'src/app/services';
import { EmploeeModel } from '../emploee.model';

@Component({
  selector: 'app-emploee-list',
  templateUrl: './emploee-list.component.html',
  styles: [``]
})
export class EmploeeListComponent implements OnInit {
    //public schema: string[] = ['id', 'fullName', 'salary', 'actions']
    public schema: any[] = [];
    public model: EmploeeModel[] = []
    public isAuth: boolean;
    public pageIndex: number = 1;

    constructor(
      public dataService: DataService, 
      jwtService: JwtService) {
        this.isAuth = jwtService.IsAuth;
      }

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
