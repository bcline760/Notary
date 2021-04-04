import { NgModule } from '@angular/core';
import { NotaryModule } from '../notary/notary.module';
import { SettingsRoutingModule } from './settings-routing.module';

import { CertificateAuthorityComponent } from './certificate-authority/certificate-authority.component';
import { SettingsHomeComponent } from './settings-home/settings-home.component';
import { SettingsComponent } from './settings.component';
import { AccountsComponent } from './accounts/accounts.component';



@NgModule({
  declarations: [CertificateAuthorityComponent, SettingsHomeComponent, SettingsComponent, AccountsComponent],
  imports: [
    NotaryModule,
    SettingsRoutingModule
  ]
})
export class SettingsModule { }
