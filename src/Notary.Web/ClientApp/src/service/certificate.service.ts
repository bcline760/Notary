import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpService } from './http.service';
import { Certificate } from '../model/certificate.model';

@Injectable({
  providedIn: 'root'
})
export class CertificateService extends DataService<Certificate> {
    constructor(protected httpSvc: HttpService) {
        super(httpSvc, "api/certificate");
    }
}
