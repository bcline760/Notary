import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { AccountDetailComponent } from './account-detail/account-detail.component';

import { AccountComponent } from './account.component';

const routes: Routes = [
  {
    path: 'account',
    component: AccountComponent,
    data: { showFullLayout: true },
    canActivate: [AuthGuardService],
    children: [
      {
        path: '',
        component: AccountDetailComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
