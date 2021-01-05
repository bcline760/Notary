import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ApiResponse } from '../model/api-response.model';
import { Data } from '../model/data.model';

/**
 * Base implementation of a service that relies on HTTP(S) interation to the server
 *
 * @typeParam T The model type of the implementing service
 */
export abstract class DataService<T extends Data> {

    constructor(protected httpSvc: HttpService, protected controller: string) { }

    protected async createAsync(model: T): Promise<ApiResponse<string>> {
        if (model === null)
            throwError('Please supply a model to create');

        let response: ApiResponse<string> = new ApiResponse<string>();

        try {
            response = await this.httpSvc.postAsync<ApiResponse<string>, T>(this.controller, model);
        } catch (e) {
            response.success = false;
            this.handleError(e);
        }

        return response;
    }

    protected async getAsync(slug: string): Promise<ApiResponse<T>> {
        const url: string = `${this.controller}/slug`;

        const response: ApiResponse<T> = await this.httpSvc.getAsync(url);

        return response;
    }

    protected async getAllAsync(): Promise<ApiResponse<T[]>> {
        const response: ApiResponse<T[]> = await this.httpSvc.getAsync(this.controller);

        return response;
    }

    protected async updateAsync(model: T): Promise<ApiResponse<string>> {
        const url: string = `${this.controller}/${model.slug}`;

        let response: ApiResponse<string> = new ApiResponse<string>();
        response.success = false;

        try {
            response = await this.httpSvc.putAsync(url, model);
        } catch (e) {
            this.handleError(e);
        }

        return response
    }

    protected async deleteAsync(slug: T): Promise<ApiResponse<string>> {
        const url: string = `${this.controller}/${slug}`;

        let response: ApiResponse<string> = new ApiResponse<string>();
        response.success = false;

        try {
            response = await this.httpSvc.deleteAsync(url);
        } catch (e) {
            this.handleError(e);
        }

        return response
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
