import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TronService {

  walletAddress: string = 'TJ5AM8CdcKKHYp6rBpfzxgWFTZvnNi1111'
  usdtContract: string = 'TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t'
  tronGridURL : string = 'https://api.trongrid.io/v1/'
  constructor(private http: HttpClient) {}


  getTransaction() {
    return this.http.get<any>(`${this.tronGridURL}accounts/${this.walletAddress}/transactions/trc20?&contract_address=${this.usdtContract}`);
  }

}
