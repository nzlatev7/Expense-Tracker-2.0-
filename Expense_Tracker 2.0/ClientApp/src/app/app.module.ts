import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddItemComponent } from './add-item/add-item.component';
import { MenuComponent } from './menu/menu.component';
import { LoginComponent } from './user/login/login.component';
import { SignupComponent } from './user/signup/signup.component';
import { ProfileComponent } from './user/profile/profile.component';
import { AdminComponent } from './user/admin/admin.component';

@NgModule({
  declarations: [
    AppComponent,
    AddItemComponent,
    MenuComponent,
    LoginComponent,
    SignupComponent,
    ProfileComponent,
    AdminComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [NgForm],
  bootstrap: [AppComponent]
})
export class AppModule { }
