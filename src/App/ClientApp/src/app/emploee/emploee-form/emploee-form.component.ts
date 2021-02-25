import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from 'src/app/login';
import { DataService, FormService } from 'src/app/services';

@Component({
  selector: 'app-emploee-form',
  templateUrl: './emploee-form.component.html',
  providers: [ FormService ],
  styles: ['']
})
export class EmploeeFormComponent implements OnInit {
    public id: any = null;
    public isAuth: boolean;

    constructor(
      public formService: FormService,
      private dataService: DataService,
      private route: ActivatedRoute,
      private router: Router,
      private jwtService: JwtService) {   
        this.isAuth = jwtService.IsAuth;     
        this.route.params.subscribe(params => { 
          this.id = params?.id > -1 ? params?.id : null; 
          //this.departmentId = params?.departmentId ?? -1; 
        })
    }

    public async ngOnInit(): Promise<void> {
      console.log('EmploeeFormComponent');
      let [schema, model] = await Promise.all([
        this.dataService.GetEmploeeSchema(),
        this.dataService.GetEmploee(this.id)
      ]);      
      this.formService.schema = schema;
      this.formService.model = model;    
    }

    public async onSubmit(model: any): Promise<void> {
      if (this.formService.form.valid != true) return;
      var result = await this.dataService.SaveEmploee(model)
        .catch((ex: any) => {
           this.formService.setErrors(ex?.error?.errors || ex);
           alert('Не удалось сохранить');
        });
      if(result && result.id > -1) {
        alert('Сохранено');
        this.router.navigate(['emploee']);
      }
    }
}
