import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Certificate } from '../contracts/certificate';
import { CertificateFormat } from '../contracts/certificate-format.enum';

@Injectable({
  providedIn: 'root'
})
export class CertificateService {

  constructor(private httpClient: HttpClient) { }

  public getCertificate(slug: string): Observable<Certificate> | null {
    const url: string = `${environment.apiUrl}/certificates/${slug}`;
    try {
      const certificate: Observable<Certificate> = this.httpClient.get<Certificate>(url).pipe(
        map(c => {
          return c;
        })
      );

      return certificate;
    } catch (e) {
      return null;
    }
  }

  public getCertificates(): Observable<Certificate[]> {
    const url: string = `${environment.apiUrl}/certificates`;

    const certificates: Observable<Certificate[]> = this.httpClient.get<Certificate[]>(url).pipe(
      map(c => {
        return c;
      })
    );

    return certificates;
  }

  downloadCertificate(slug: string, format: CertificateFormat, keyPassword?: string): Observable<any> {
    throw new Error('Not implemented right now.');
  }
}