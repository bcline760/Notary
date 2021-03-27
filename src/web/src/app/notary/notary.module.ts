import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageBoxComponent } from './message-box/message-box.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [MessageBoxComponent],
  imports: [
    CommonModule
  ],
  exports: [MessageBoxComponent, CommonModule, FormsModule]
})
export class NotaryModule { }
