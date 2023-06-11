import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AdminComponent } from '../admin.component';
import { AdminService } from 'src/app/core/services/admin.service';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.scss']
})
export class UpdateComponent implements OnInit {

  constructor(private admin: AdminService) { }

  ngOnInit(): void {
    this.admin.changeInfo$.subscribe({
      next: resp => {
        this.form.patchValue({
          "id": resp.id,
          "userName": resp.userName,
          "password": resp.password,
          "role": resp.role,
          "email": resp.email
        })
      }
    })
  }

  form = new FormGroup({
    id: new FormControl(0),
    userName: new FormControl(''),
    password: new FormControl(''),
    role: new FormControl(0),
    email: new FormControl('')
  });

  onSubmit() {
    const body = {
      "id": this.form.value.id,
      "userName": this.form.value.userName,
      "password": this.form.value.password,
      "role": this.form.value.role,
      "email": this.form.value.email
    }
    this.admin.updateUser(body).subscribe({
      next: resp => console.log(resp),
      error: error => console.log(error)
    })
  }

}
