import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Certificate } from 'src/contract/certificate.contract';
import { CertificateService } from 'src/service/certificate.service';
import { SessionService } from 'src/service/session.service';

@Component({
  selector: 'app-certificate-authority',
  templateUrl: './certificate-authority.component.html',
  styleUrls: ['./certificate-authority.component.scss']
})
export class CertificateAuthorityComponent implements OnInit {

  constructor(private certSvc: CertificateService, private sessionSvc: SessionService) { }

  private _rootCertificate: Certificate | null;
  private _intermediateCertificate: Certificate | null;

  ngOnInit(): void {
    this.certSvc.getCertificates().pipe(first()).subscribe(c => {
      c.forEach(cc => {
        if (!cc.isPrimarySigning && cc.issuer == cc.subject) {
          this._rootCertificate = cc;
        } else if (cc.isPrimarySigning && cc.issuer != cc.subject) {
          this._intermediateCertificate = cc;
        }
      })
    });
  }

  get rootCertificate() { return this._rootCertificate; }
  get intermediateCertificate() { return this._intermediateCertificate; }
}
