import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SessionService } from 'src/service/session.service';
import { AuthenticatedUser } from 'src/contract/authenticated-user.contract';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private sessionService: SessionService, private router: Router) {

    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const apiToken: AuthenticatedUser | null = this.sessionService.currentAuthenticatedUser;
        const isLoggedOn: string | null = apiToken && apiToken.token;
        const isApiUrl = req.url.startsWith(environment.apiUrl);

        // Check to see if logged on and has a valid token
        if (isLoggedOn && isApiUrl) {
            if (this.sessionService.isValidSession()) {
                req = req.clone({
                    setHeaders: {
                        Authorization: `Bearer ${apiToken?.token}`
                    }
                });
            } else {
                this.sessionService.signOut();
                this.router.navigate(['session', 'signin']);
            }
        }
        return next.handle(req);
    }
}