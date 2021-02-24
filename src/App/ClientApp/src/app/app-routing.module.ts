import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmploeeFormComponent, EmploeeListComponent } from './emploee';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';

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
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '**',
    redirectTo: 'login', 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule],
  providers: [], 
})
export class AppRoutingModule { }
