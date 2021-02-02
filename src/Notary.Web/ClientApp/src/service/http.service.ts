import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http'
import { Observable, pipe, zip, range, throwError, of, timer } from 'rxjs';
import { map, mergeMap, retryWhen } from 'rxjs/operators';
import { TokenService } from './token.service';

@Injectable({
    providedIn: 'root'
})
export class HttpService {

    constructor(private http: HttpClient, private tokenSvc: TokenService) {
        this.isCrossOrigin = false;
    }

    /**
     * Get or set this to inform that this is a cross-origin request and set the header
     */
    public isCrossOrigin: boolean;

    /**
     * Get data from the REST API
     * @param url The absolute URL to the REST API
     */
    getAsync<R>(url: string): Observable<R> {
        return this._executeHttpMethod(url, "GET");
    }

    /**
     * Send a HTTP PUT to the REST API. This is typically updating data.
     * @param url The absolute URL to the REST API
     * @param body The request body to send
     */
    putAsync<R, B>(url: string, body: B): Observable<R> {
        return this._executeHttpMethod(url, "PUT", body);
    }

    /**
     * Send a HTTP POST to the REST API. This is typically for creating data
     * @param url The absolute URL to the REST API
     * @param body The request body to send
     */
    postAsync<R, B>(url: string, body: B): Observable<R> {
        return this._executeHttpMethod(url, "POST", body);
    }

    /**
     * Execute a HTTP DELETE on the REST API
     * @param url The absolute URL to the REST API
     */
    deleteAsync<R>(url: string): Observable<R> {
        return this._executeHttpMethod(url, "DELETE");
    }

    private _executeHttpMethod<R, B>(url: string, method: "GET" | "PUT" | "POST" | "DELETE", body?: B): Observable<R> {
        let httpHeaders: HttpHeaders = new HttpHeaders();
        httpHeaders = httpHeaders.append('Content-Type', 'application/json');

        const token: string | null = this.tokenSvc.getToken();
        if (token) {
            httpHeaders = httpHeaders.append('Authorization', `Bearer ${token}`);
        }

        let data: Observable<R> = new Observable<R>();
        switch (method) {
            case "GET":
                data = this.http.get<R>(url, { headers: httpHeaders });
                break;
            case "PUT":
                data = this.http.put<R>(url, body, { headers: httpHeaders });
                break;
            case "POST":
                data = this.http.post<R>(url, body, { headers: httpHeaders });
                break;
            case "DELETE":
                data = this.http.delete<R>(url, { headers: httpHeaders });
                break;
        }

        return data;
    }
}
