import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { Data } from '../model/data.model';

/**
 * Base implementation of a service that relies on HTTP(S) interation to the server
 *
 * @typeParam T The model type of the implementing service
 */
export abstract class DataService<T extends Data> {

    constructor(protected httpSvc: HttpService, protected controller: string) { }

    protected createAsync(model: T): Observable<string> {
        if (model === null)
            throwError('Please supply a model to create');

        const response: Observable<string> = this.httpSvc.postAsync<string, T>(this.controller, model);

        return response
    }

    protected getAsync(slug: string): Observable<T> {
        const url: string = `${this.controller}/slug`;

        const response: Observable<T> = this.httpSvc.getAsync(url);

        return response;
    }

    protected getAllAsync(): Observable<T[]> {
        const response: Observable<T[]> = this.httpSvc.getAsync(this.controller);

        return response;
    }

    protected updateAsync(model: T): Observable<string> {
        if (model === null)
            throwError('Please supply a model to update');

        const url: string = `${this.controller}/${model.slug}`;

        const response: Observable<string> = this.httpSvc.putAsync(url, model);

        return response;
    }

    protected deleteAsync(slug: string): Observable<string> {
        const url: string = `${this.controller}/${slug}`;

        const response: Observable<string> = this.httpSvc.deleteAsync(slug);

        return response
    }
}
