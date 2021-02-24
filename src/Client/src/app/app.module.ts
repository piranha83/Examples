import { BrowserModule } from '@angular/platform-browser'
import { NgModule } from '@angular/core'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatButtonModule } from '@angular/material/button'
import { HttpClientModule } from '@angular/common/http'
import { ReactiveFormsModule } from '@angular/forms'
import { FormlyModule } from '@ngx-formly/core'
import { FormlyMaterialModule } from '@ngx-formly/material'
import { FormlyMatDatepickerModule } from '@ngx-formly/material/datepicker'
import { FormlyMatToggleModule } from '@ngx-formly/material/toggle'
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatDialogModule } from '@angular/material/dialog'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatRadioModule } from '@angular/material/radio'
import { MatSelectModule } from '@angular/material/select'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatNativeDateModule } from '@angular/material/core'
import { MatTableModule } from '@angular/material/table';
import {MatListModule} from '@angular/material/list';
import { EmploeeFormComponent, EmploeeListComponent } from './emploee'
import { AppConfig } from './app.config';
import { LoginComponent } from './login/login.component'

@NgModule({
    declarations: [
        AppComponent,
        EmploeeListComponent,
        EmploeeFormComponent,
        LoginComponent,
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatButtonModule,
        ReactiveFormsModule,
        FormlyModule.forRoot(AppConfig),
        FormlyMaterialModule,
        ReactiveFormsModule,
        MatCheckboxModule,
        MatDatepickerModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatRadioModule,
        MatSelectModule,
        MatTableModule,
        MatNativeDateModule,
        MatListModule,
        FormlyMatDatepickerModule,
        FormlyMatToggleModule,
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}