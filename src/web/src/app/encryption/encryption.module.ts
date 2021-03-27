import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EncryptionRoutingModule } from './encryption-routing.module';
import { EncryptionComponent } from './encryption.component';


@NgModule({
  declarations: [EncryptionComponent],
  imports: [
    CommonModule,
    EncryptionRoutingModule
  ]
})
export class EncryptionModule { }
