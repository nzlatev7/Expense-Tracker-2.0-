import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  constructor(private httpUser: UserService) { }

  ngOnInit(): void {
    this.getAllUsers();
  }

  items: any = [];

  getAllUsers(){
    this.httpUser.getAll().subscribe({
      next: (resp) => this.items = resp,
      error: (err) => console.log(err)
    });
  }
}
