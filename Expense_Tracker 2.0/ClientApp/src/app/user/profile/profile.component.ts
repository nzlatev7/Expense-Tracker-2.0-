import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(private httpUser: UserService) { }

  ngOnInit(): void {
    this.asd();
  }

  items: any = [];

  asd(){
    this.httpUser.getAll().subscribe({
      next: (resp) => this.items = resp,
      error: (err) => console.log(err)
    })
  }

}
