import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { LoadingComponent } from './loading/loading.component';
import { LoadingService } from '../services/loading.service';



@NgModule({
  declarations: [
    NavMenuComponent,
    LoadingComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    LoadingComponent, NavMenuComponent
  ],
  providers: [LoadingService]
})
export class LayoutModule { }
