import { Component, OnInit } from '@angular/core';
import { CertificateService } from 'src/service/certificate.service';
import { SessionService } from 'src/service/session.service';

@Component({
  selector: 'app-certificate-list',
  templateUrl: './certificate-list.component.html',
  styleUrls: ['./certificate-list.component.scss']
})
export class CertificateListComponent implements OnInit {

  constructor(private certSvc: CertificateService, private sessionSvc: SessionService) { }

  ngOnInit(): void {
  }

}
