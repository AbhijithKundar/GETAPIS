import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { TronService } from './services/tron.service';
import { TronTransactionModel } from './models/tron.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit  {
  title = 'AngularAuthYtUI';
  showMenu: boolean = false;
  isUserLoggedIn: boolean = false;
  tronModel : TronTransactionModel = new TronTransactionModel();
  userName?: string;

constructor(private auth : AuthService, private tronSrvc: TronService) {
}

ngOnInit(): void {
  this.isUserLoggedIn = this.auth.isLoggedIn();
  if(this.isUserLoggedIn)
  {
this.userName = this.auth.decodedToken().name;
  }
  this.tronSrvc.getTransaction().subscribe(res=> {
    this.tronModel = res;
  });
}
  showSideBarMenus() {
    this.showMenu = !this.showMenu;
  }

  hideMenu() {
    this.showMenu = false;
  }

  
  logout(){
    this.auth.signOut();
  }
}
