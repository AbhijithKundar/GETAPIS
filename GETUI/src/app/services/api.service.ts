import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseURLService } from './base-url.service';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl: string = this.baseUrlService.baseURL;
  constructor(private http: HttpClient, private baseUrlService: BaseURLService) {}

  getUsers() {
    return this.http.get<any>(`${this.baseUrl}user`);
  }

  getMembersOnUserName(name: string) {
    return this.http.get<any>(`${this.baseUrl}Member/getMembersForUser?userName=${name}`);
  }

  getDashboardOnUserName(name: string) {
    return this.http.get<any>(`${this.baseUrl}Dashboard/GetDashboard?userName=${name}`);
  }
  
  getTeamTree(name: string) {
    return this.http.get<any>(`${this.baseUrl}Member/getTeamTree?userName=${name}`);
  }
}
