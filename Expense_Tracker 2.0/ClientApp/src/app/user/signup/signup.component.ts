import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  constructor(private user: UserService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm){
    const body={
      "userName": form.value.username,
      "password": form.value.password,
      "email": form.value.mail
    }
    this.user.register(body).subscribe({
      next: (resp) => console.log(resp),
      error: (error) => console.log(error)
    });
  }

}
