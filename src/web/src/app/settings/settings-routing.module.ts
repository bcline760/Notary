import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Role } from 'src/contract/role.enum';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { AccountsComponent } from './accounts/accounts.component';
import { CertificateAuthorityComponent } from './certificate-authority/certificate-authority.component';
import { SettingsHomeComponent } from './settings-home/settings-home.component';
import { SettingsComponent } from './settings.component';


const routes: Routes = [
  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [AuthGuardService],
    data: {
      showFullLayout: true,
      roles: [Role.Admin]
    },
    children: [
      {
        path: 'accounts',
        component: AccountsComponent
      },
      {
        path: 'certificate-authority',
        component: CertificateAuthorityComponent
      },
      {
        path: '',
        component: SettingsHomeComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
