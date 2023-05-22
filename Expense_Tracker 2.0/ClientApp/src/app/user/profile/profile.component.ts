import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(private user: UserService) { }

  ngOnInit(): void {
    // this.user.getInfo().subscribe({
    //   next: resp => console.log(resp),
    //   error: error => console.log(error)
    // })
  }



}
