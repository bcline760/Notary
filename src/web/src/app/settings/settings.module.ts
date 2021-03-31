import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { CertificateAuthorityComponent } from './certificate-authority/certificate-authority.component';
import { SettingsHomeComponent } from './settings-home/settings-home.component';
import { SettingsComponent } from './settings.component';
import { AccountsComponent } from './accounts/accounts.component';


@NgModule({
  declarations: [CertificateAuthorityComponent, SettingsHomeComponent, SettingsComponent, AccountsComponent],
  imports: [
    CommonModule,
    SettingsRoutingModule
  ]
})
export class SettingsModule { }
