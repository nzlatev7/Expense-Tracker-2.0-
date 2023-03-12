import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

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
    this.user.register(form.value).subscribe({
      next: (resp) => console.log(resp),
      error: (error) => console.log(error)
    });
  }

}
