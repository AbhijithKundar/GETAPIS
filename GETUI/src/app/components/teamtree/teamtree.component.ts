import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamTreeModel } from 'src/app/models/member.model';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-teamtree',
  templateUrl: './teamtree.component.html',
  styleUrls: ['./teamtree.component.scss']
})
export class TeamtreeComponent implements OnInit {
  teamTreeModel: TeamTreeModel[] = [];
  memberName?: string;

  constructor(private api: ApiService, private auth: AuthService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.memberName = this.route.snapshot.paramMap.get('userName')?.toString();
    if (this.memberName != null) {
    this.api.getTeamTree(this.memberName).subscribe(res => {
      this.teamTreeModel = res;
    });
  }

  }

}
