import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

//Shamelessly ganked from: https://nezhar.com/blog/create-a-loading-screen-for-angular-apps/
@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private _loading: boolean = false;
  public loadingStatus: Subject<boolean> = new Subject<boolean>();

  constructor() { }

  get loading(): boolean {
    return this._loading;
  }

  set loading(value: boolean) {
    this._loading = value;
    this.loadingStatus.next(value);
  }

  public startLoading() {
    this.loading = true;
  }

  public stopLoading() {
    this.loading = false;
  }
}
