import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ApiResponse } from '../model/api-response.model';

/**
 * Base implementation of a service that relies on HTTP(S) interation to the server
 *
 * @typeParam T The model type of the implementing service
 */
@Injectable({
    providedIn: 'root'
})
export abstract class DataService<T> {

    constructor(protected httpSvc: HttpService, protected controller: string) { }

    /**
     * Get data from the server controller
     * @param method The server controller method
     */
    protected async getFromServerAsync(method: string): Promise<ApiResponse<T>> {
        const url = `${this.controller}/${method}`;
        return this.httpSvc.getAsync(method);
    }

    protected handleError(error: HttpErrorResponse): void {
        if (error.error instanceof ErrorEvent) {
            console.error('An error has occured:', error.message);
            throwError('An error has occured within the application');
        } else {
            console.error('API server returned', error.status);
        }
    }
}
