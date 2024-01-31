import { Component, OnInit } from '@angular/core';
import { MemberModel, TeamTree } from 'src/app/models/member.model';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.scss']
})
export class MemberComponent implements OnInit {
  teamTree: TeamTree[] = [];
  memberName?: string;
  isOnLoad: boolean = true;
  constructor(private api: ApiService, private auth: AuthService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.memberName = this.route.snapshot.paramMap.get('userName')?.toString();
    if (this.memberName != null) {
      this.getMember(this.memberName)
      this.isOnLoad = true;

    }
  }

  getMember(memberName: string) {
    this.api.getMembersOnUserName(memberName).subscribe(res => {
      this.teamTree = res;
    });
    this.isOnLoad = false
  }

  backClicked() {
    if (this.memberName) {
      this.getMember(this.memberName);
      this.isOnLoad = true;
    }
  }

}
