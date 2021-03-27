import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { CertificateDetailComponent } from './certificate-detail/certificate-detail.component';

import { CertificatesComponent } from './certificates.component';

const routes: Routes = [
  {
    path: ':thumb',
    component: CertificateDetailComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: '',
    component: CertificatesComponent,
    canActivate: [AuthGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CertificatesRoutingModule { }
