import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CertificatesRoutingModule } from './certificates-routing.module';
import { CertificatesComponent } from './certificates.component';
import { CertificateListComponent } from './certificate-list/certificate-list.component';
import { CertificateDetailComponent } from './certificate-detail/certificate-detail.component';


@NgModule({
  declarations: [CertificatesComponent, CertificateListComponent, CertificateDetailComponent],
  imports: [
    CommonModule,
    CertificatesRoutingModule
  ]
})
export class CertificatesModule { }
