import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddItemComponent } from './add-item/add-item.component';
import { MenuComponent } from './menu/menu.component';
import { LoginComponent } from './user/login/login.component';
import { SignupComponent } from './user/signup/signup.component';
import { ProfileComponent } from './user/profile/profile.component';
import { AdminComponent } from './user/admin/admin.component';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { ErrorPageComponent } from './error-page/error-page.component';
import { HomePageComponent } from './home-page/home-page.component';
import { AllUsersComponent } from './user/admin/all-users/all-users.component';
import { StepByStepComponent } from './user/admin/step-by-step/step-by-step.component';
import { UpdateComponent } from './user/admin/update/update.component';

@NgModule({
  declarations: [
    AppComponent,
    AddItemComponent,
    MenuComponent,
    LoginComponent,
    SignupComponent,
    ProfileComponent,
    AdminComponent,
    ErrorPageComponent,
    HomePageComponent,
    AllUsersComponent,
    StepByStepComponent,
    UpdateComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    NgForm,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
