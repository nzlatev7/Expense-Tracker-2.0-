import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.scss']
})
export class UpdateComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  form = new FormGroup({
    id: new FormControl(''),
    userName: new FormControl(''),
    possword: new FormControl(''),
    role: new FormControl(''),
    email: new FormControl('')
  });

}
