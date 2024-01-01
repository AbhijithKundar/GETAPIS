import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit  {
  title = 'AngularAuthYtUI';
  showMenu: boolean = false;
  isUserLoggedIn: boolean = false;

constructor(private auth : AuthService) {
}

ngOnInit(): void {
  this.isUserLoggedIn = this.auth.isLoggedIn();
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
