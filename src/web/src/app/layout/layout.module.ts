import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { LoadingComponent } from './loading/loading.component';
import { LoadingService } from 'src/service/loading.service';
import { MenuComponent } from './menu/menu.component';


@NgModule({
  declarations: [LoadingComponent, MenuComponent],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule
  ],
  exports: [
    LoadingComponent,
    MenuComponent
  ],
  providers: [LoadingService]
})
export class LayoutModule { }
