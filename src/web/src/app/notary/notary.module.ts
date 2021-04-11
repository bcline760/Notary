import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageBoxComponent } from './message-box/message-box.component';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [MessageBoxComponent],
  imports: [
    CommonModule,
    NgbModule
  ],
  exports: [MessageBoxComponent, CommonModule, FormsModule]
})
export class NotaryModule { }
