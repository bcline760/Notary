import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SessionComponent } from './session.component';
import { SigninComponent } from './signin/signin.component';
import { SignoutComponent } from './signout/signout.component';


const routes: Routes = [
  {
    path: 'session',
    component: SessionComponent,
    data: { showFullLayout: false },
    children: [
      {
        path: 'signin',
        component: SigninComponent
      },
      {
        path: 'signout',
        component: SignoutComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SessionRoutingModule { }
