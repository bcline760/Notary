import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { Certificate } from 'src/contract/certificate.contract';
import { CertificateService } from 'src/service/certificate.service';
import { SessionService } from 'src/service/session.service';
import { CertificateAuthoritySetup } from '../../../contract/certificate-authority-setup.contract';
import { CaSetup } from '../../../model/ca-setup.model';

@Component({
  selector: 'app-certificate-authority',
  templateUrl: './certificate-authority.component.html',
  styleUrls: ['./certificate-authority.component.scss']
})
export class CertificateAuthorityComponent implements OnInit {

  constructor(private certSvc: CertificateService, private sessionSvc: SessionService) {
    this._caSetupModel = new CaSetup();
  }

  private _rootCertificate: Certificate | null = null;
  private _intermediateCertificate: Certificate | null = null;
  private _caSetupModel: CaSetup;

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

    this.caForm = new FormGroup({
      rootCommonName: new FormControl(this.caSetup.rootCommonName, [
        Validators.required,
      ]),
      interCommonName: new FormControl(this.caSetup.intermediateCommonName, [
        Validators.required
      ]),
      organization: new FormControl(this.caSetup.organization, []),
      organizationalUnit: new FormControl(this.caSetup.organizationalUnit, []),
      locale: new FormControl(this.caSetup.locale, []),
      province: new FormControl(this.caSetup.stateProvince, []),
      country: new FormControl(this.caSetup.country),
      keyLength: new FormControl(this.caSetup.keyLength, [
        Validators.required,
        Validators.min(0)
      ]),
      lengthInYears: new FormControl(this.caSetup.expiryInYears, [
        Validators.required,
        Validators.min(0)
      ])
    });
  }

  public setupCertificateAuthority(): void {

  }

  public caForm: FormGroup;

  get caSetup() { return this._caSetupModel; }
  get rootCertificate() { return this._rootCertificate; }
  get intermediateCertificate() { return this._intermediateCertificate; }
}
