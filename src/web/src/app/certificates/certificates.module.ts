import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CertificatesRoutingModule } from './certificates-routing.module';
import { CertificatesComponent } from './certificates.component';
import { CertificateDetailComponent } from './certificate-detail/certificate-detail.component';
import { NotaryModule } from '../notary/notary.module';


@NgModule({
  declarations: [CertificatesComponent, CertificateDetailComponent],
  imports: [
    NotaryModule,
    CertificatesRoutingModule
  ]
})
export class CertificatesModule { }
