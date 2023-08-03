import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private user: UserService,
              private router: Router) {
   }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    const body = {
      userName: form.value.username,
      password: form.value.password
    }
    this.user.logIn(body).subscribe({
      next: (token) => {
        if (token !== null) {
          localStorage.setItem('token', JSON.stringify(token));
          this.user.checkToken();
        }
        this.router.navigate(['/add-new-item']);
      },
      error: (error) => console.log(error)
    });
  }

}
