import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
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

  onUpdate: boolean = false;

  userInfo: any = {
    userName: "",
    password: "",
    email: ""
  }

  form = new FormGroup({
    userName: new FormControl(''),
    password: new FormControl(''),
    email: new FormControl('')
  })

  logOut(): void {
    const token = localStorage.getItem('token');
    localStorage.removeItem('token');
    if (token) {
      this.router.navigate(['/home']);
      this.user.checkToken();
    }
  }

  onSubmit(): void {
    const body = {
      userName: this.form.value.userName,
      password: this.form.value.password,
      email: this.form.value.email
    }
    this.user.update(body).subscribe({
      next: resp => console.log(resp),
      error: err => console.log(err)
    })
    this.back();
  }

  back(): void {
    this.onUpdate = false;
  }

  change(): void {
    this.onUpdate = true;
    this.form.patchValue({
      userName: this.userInfo.userName,
      password: this.userInfo.password,
      email: this.userInfo.email
    })
  }

  deleteUser(): void {
    this.user.deleteItem().subscribe({
      next: resp => console.log(resp),
      error: err => console.log(err)
    });
    this.logOut();
  }

}
