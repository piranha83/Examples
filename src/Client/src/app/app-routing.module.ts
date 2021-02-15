import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmploeeFormComponent, EmploeeListComponent } from './emploee';

const routes: Routes = [
  {
      path: 'emploee',
      component: EmploeeListComponent,
  },
  {
      path: 'emploee/:id',
      component: EmploeeFormComponent,
  },
  {
    path: '**',
    redirectTo: 'emploee',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [], 
})
export class AppRoutingModule { }
