import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SignupComponent } from './components/signup/signup.component';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { MemberComponent } from './components/member/member.component';
import { TeamtreeComponent } from './components/teamtree/teamtree.component';

const routes: Routes = [
  {path:'login', component:LoginComponent},
  {path:'signup', component:SignupComponent},
  {path:'dashboard', component:DashboardComponent, canActivate:[AuthGuard]},
  {path:'member/:userName', component:MemberComponent, canActivate:[AuthGuard]},
  {path:'teamtree/:userName', component:TeamtreeComponent, canActivate:[AuthGuard]},
  {path:'', redirectTo:'app-root', pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
