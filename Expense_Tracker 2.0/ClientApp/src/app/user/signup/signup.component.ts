import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  constructor(private user: UserService,
              private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm){
    const body={
      "userName": form.value.username,
      "password": form.value.password,
      "email": form.value.mail
    }
    this.user.register(body).subscribe({
      next: (resp) => this.router.navigate(['log-in']),
      error: (error) => console.log(error)
    });
  }

}
