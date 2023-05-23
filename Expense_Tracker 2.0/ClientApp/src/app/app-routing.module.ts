import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddItemComponent } from './add-item/add-item.component';
import { AdminComponent } from './user/admin/admin.component';
import { LoginComponent } from './user/login/login.component';
import { ProfileComponent } from './user/profile/profile.component';
import { SignupComponent } from './user/signup/signup.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { AuthGuardService } from './core/guards/auth-guard.service';
import { HomePageComponent } from './home-page/home-page.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'add-new-item', canActivate:[AuthGuardService], component: AddItemComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'sign-up', component: SignupComponent},
  { path: 'log-in', component: LoginComponent},
  { path: 'my-profile', canActivate:[AuthGuardService], component: ProfileComponent},
  { path: 'admin', canActivate:[AuthGuardService], component: AdminComponent},
  { path: 'error', component: ErrorPageComponent},
  { path: '**', redirectTo: 'error' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
