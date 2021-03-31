import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { CertificateService } from 'src/service/certificate.service';

@Component({
  selector: 'app-home-display',
  templateUrl: './home-display.component.html',
  styleUrls: ['./home-display.component.scss']
})
export class HomeDisplayComponent implements OnInit {

  constructor(private certSvc: CertificateService) { }

  private _totalCerts: number = 0;
  private _expiringCerts: number = 0;
  private _expiredCerts: number = 0;
  private _revokedCerts: number = 0;

  ngOnInit(): void {
    this._loadCertificates();
  }

  private _loadCertificates(): void {
    const now: Date = new Date();
    const window: Date = new Date(now.getTime() + ((1000 * 60 * 60 * 24) * 30));
    this.certSvc.getCertificates().pipe(first()).subscribe(c => {
      c.forEach(cc => {
        // We only care about issued certificates, not the root certs
        if (cc.signingCertSlug == null) {
          this._totalCerts++;

          if (cc.notAfter > now) {
            this._expiredCerts++;
          } else if (cc.notAfter <= now && cc.notAfter >= window) {
            this._expiringCerts++;
          } else if (cc.revokeDate != null) {
            this._revokedCerts++;
          }
        }
      })
    });
  }

  get totalCerts(): number { return this._totalCerts; }

  get expiringCerts(): number { return this._expiringCerts; }

  get expiredCerts(): number { return this._expiredCerts; }

  get revokedCerts(): number { return this._revokedCerts; }
}
