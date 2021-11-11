import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private config: any;

  constructor(private http: HttpClient) { }

  getConfiguration = () => this.config;
  
  async loadConfiguration(location: string) {
    this.config = await firstValueFrom(this.http.get(location));
  }
}
