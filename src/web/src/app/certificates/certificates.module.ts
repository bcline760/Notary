import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CertificatesRoutingModule } from './certificates-routing.module';
import { CertificatesComponent } from './certificates.component';
import { CertificateDisplayComponent } from './certificate-display/certificate-display.component';


@NgModule({
  declarations: [CertificatesComponent, CertificateDisplayComponent],
  imports: [
    CommonModule,
    CertificatesRoutingModule
  ]
})
export class CertificatesModule { }
