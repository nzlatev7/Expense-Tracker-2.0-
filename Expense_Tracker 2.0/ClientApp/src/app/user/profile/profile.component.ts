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
    this.user.getInfo().subscribe({
      next: resp => this.userInfo = resp,
      error: error => console.log(error)
    })
  }

  password: boolean = false;

  showPassword(): void {
    this.password = true;
  }

  hidePassword(): void {
    this.password = false;
  }

  userInfo: any = {
    userName: "",
    password: "",
    email: ""
  }

  logOut(): void {
    const token = localStorage.getItem('token');
    localStorage.removeItem('token');
    if (token) {
      this.router.navigate(['/home']);
      this.user.checkToken();
    }
  }

  updateUser():void{
    
  }

  deleteUser():void{
    this.user.deleteItem().subscribe({
      next: resp => console.log(resp),
      error: err => console.log(err)
    });
    this.logOut();
  }

}
