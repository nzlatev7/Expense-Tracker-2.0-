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
  }

  form = new FormGroup({
    id: new FormControl(0),
    userName: new FormControl(''),
    password: new FormControl(''),
    role: new FormControl(0),
    email: new FormControl('')
  });

  onSubmit() {
    
  }

}
