import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(private user: UserService,
              private router: Router) { }

  ngOnInit(): void {
    // this.user.getInfo().subscribe({
    //   next: resp => console.log(resp),
    //   error: error => console.log(error)
    // })
  }

  logOut(){
    const token = localStorage.getItem('token');
    localStorage.removeItem('token');
    if (token) {
      this.router.navigate(['/home']);
      this.user.checkToken();
    }
  }

}
