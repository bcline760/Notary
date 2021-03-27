import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Certificate } from 'src/contract/certificate.contract';
import { CertificateService } from 'src/service/certificate.service';
import { SessionService } from 'src/service/session.service';

@Component({
  selector: 'app-certificates',
  templateUrl: './certificates.component.html',
  styleUrls: ['./certificates.component.scss']
})
export class CertificatesComponent implements OnInit {

  constructor(private certSvc: CertificateService, private sessionSvc: SessionService) { }

  ngOnInit(): void {
    this.certificates = this.certSvc.getCertificates();
  }

  openCertificate(thumb: string): void {

  }

  deleteCertificate(thumb: string): void {

  }

  public certificates: Observable<Certificate[]>
}
