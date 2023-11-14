import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment.development';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root',
})
export class EmployeeServiceService {
  baseURL = env.baseURL;
  constructor(private http: HttpClient) {}
  GetAll() {
    return this.http.get(`${this.baseURL}Data/GetAll`);
  }
}
