import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResetPassword } from '../models/reseet-password.model';
import { BaseURLService } from './base-url.service';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {

  constructor(private http: HttpClient, private baseUrlService: BaseURLService) { }
  private baseUrl = this.baseUrlService.baseURL;

  sendPasswordLinlk(email: string)
  {
    return this.http.post<any>(`${this.baseUrl}user/send-reset-email/${email}`, {});
  }

  resetPassword(resetPasswordObj: ResetPassword)
  {
    return this.http.post<any>(`${this.baseUrl}user/reset-password`, resetPasswordObj)
  }
}
