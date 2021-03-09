import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingService } from 'src/service/loading.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  _activeRequests: number = 0;
  _skipUrls: string[] = [

  ];

  constructor(private loadingSvc: LoadingService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let displayLoading: boolean = true;
    for (const url of this._skipUrls) {
      if (new RegExp(url).test(request.url)) {
        displayLoading = false;
        break;
      }
    }

    if (displayLoading) {
      if (this._activeRequests === 0) {
        this.loadingSvc.startLoading();
      }
      this._activeRequests++;


      return next.handle(request).pipe(
        finalize(() => {
          this._activeRequests--;
          if (this._activeRequests === 0) {
            this.loadingSvc.stopLoading();
          }
        })
      );
    } else {
      return next.handle(request);
    }
  }
}
