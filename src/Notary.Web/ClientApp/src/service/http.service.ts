import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'

/**
 * Service implementation that performs basic RESTful procedures over HTTP(S)
 *
 * @usageNotes
 * ```
 * const url:string = 'session/login';
 * const response = await this.httpSvc.postAsync(url, credentials);
 * ```
 */
@Injectable({
    providedIn: 'root'
})
export class HttpService {
    constructor(protected http: HttpClient) { }

    /**
     * Post data to the server. This is "create" for REST calls
     * @param url The URL of the API method
     * @param body The data with which to send to the server
     */
    async postAsync<R, B>(url: string, body: B): Promise<R> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        return await this.http.post<R>(url, body, httpOptions).toPromise();
    }

    /**
     * Get data from the server by HTTP GET method
     * @param url The API method with which to retrieve data
     */
    async getAsync<R>(url: string): Promise<R> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        return await this.http.get<R>(url, httpOptions).toPromise();
    }

    /**
     * Put date to the server. This is usually used for "updating" data
     * @param url The URL of the API method
     * @param body The data contents
     */
    async putAsync<R, B>(url: string, body: B): Promise<R> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };

        return await this.http.put<R>(url, body, httpOptions).toPromise();
    }

    /**
     * Delete data from the server.
     * @param url The API delete method URL
     */
    async deleteAsync<R>(url: string): Promise<R> {
        return await this.http.delete<R>(url).toPromise();
    }
}
