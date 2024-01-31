import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BaseURLService {
  //baseURL: string = "https://localhost:7058/api/"
 baseURL: string = "https://getappapi.azurewebsites.net/api/"
  constructor() { }

}
