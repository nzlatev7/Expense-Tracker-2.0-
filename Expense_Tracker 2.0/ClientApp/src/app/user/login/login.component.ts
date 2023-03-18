import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private user: UserService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm){
    this.user.logIn(form.value).subscribe({
      next: (resp) => console.log(resp),
      error: (error) => console.log(error)
    });
  }

}
