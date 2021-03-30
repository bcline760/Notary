import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/service/auth-guard.service';
import { CertificateDetailComponent } from './certificate-detail/certificate-detail.component';
import { CertificateListComponent } from './certificate-list/certificate-list.component';
import { CertificatesComponent } from './certificates.component';

const routes: Routes = [
  {
    path: 'certificates',
    component: CertificatesComponent,
    canActivate: [AuthGuardService],
    data: { showFullLayout: true },
    children: [
      {
        path: ':thumb',
        component: CertificateDetailComponent
      },
      {
        path: '',
        component: CertificateListComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CertificatesRoutingModule { }
