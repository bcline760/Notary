import { Injectable } from '@angular/core';
import { Observable, forkJoin, of } from 'rxjs';
import { tap, switchMap, concatMap } from 'rxjs/operators';

import { HttpService } from './http.service';
import { TokenService } from './token.service';

import { ApiToken } from '../model/api-token';
import { Account } from '../model/account.model';

@Injectable({
    providedIn: 'root'
})
export class SessionService {
    constructor(private httpSvc: HttpService, private tokenSvc: TokenService) {

    }

    signInAsync(key: string, secret: string): Observable<Account | null> {
        const path: string = 'session/signin';

        const post = {
            username: key,
            password: secret,
            persist: false
        };

        return this.httpSvc.postAsync<ApiToken, any>(path, post)
            .pipe(switchMap(t => {
                if (t && t.active) {
                    this.tokenSvc.setToken(t.token);
                    return this.httpSvc.getAsync<Account>(`account/${t.accountSlug}`);
                }

                return of(null);
            }));
    }
}
