import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ConfigService {
    private config: any;

    constructor(private http: HttpClient) { }

    getConfiguration = () => this.config;

    loadConfiguration = (location: string) => 
        this.http.get(location)
            .toPromise()
            .then(config => this.config = config);
}