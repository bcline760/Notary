import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SessionComponent } from './session.component';
import { SigninComponent } from './signin/signin.component';

const routes: Routes = [
  {
    path: '', component: SessionComponent,
    children: [
      {
        path: 'signin',
        component: SigninComponent,
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SessionRoutingModule { }
