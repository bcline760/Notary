import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../services/auth-guard.service';
import { SessionComponent } from './session.component';
import { SigninComponent } from './signin/signin.component';
import { SignoutComponent } from './signout/signout.component';

const routes: Routes = [{
  path: 'session',
  component: SessionComponent,
  children: [
    {
      path: 'signin',
      component: SigninComponent,
      data: {
        showFullLayout: true
      }
    },
    {
      path: 'signout',
      component: SignoutComponent,
      canActivate: [AuthGuardService],
      data: {
        showFullLayout: false
      }
    }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SessionRoutingModule { }
