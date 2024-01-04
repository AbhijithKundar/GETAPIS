import { Component, OnInit } from '@angular/core';
import { MemberModel } from 'src/app/models/member.model';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.scss']
})
export class MemberComponent implements OnInit {
 members :MemberModel[] = [];

  constructor(private api : ApiService, private auth: AuthService) { }

  ngOnInit(): void {
    this.api.getMembersOnUserName(this.auth.getfullNameFromToken()).subscribe(res=>{
      this.members = res;
      });
  }

}
