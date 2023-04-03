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

  users: any = [];

  getAllUsers(){
    this.httpUser.getAll().subscribe({
      next: (resp) => this.users = resp,
      error: (err) => console.log(err)
    });
  }

  deleteUser(id: number){
    const body = {
      id: id
    }
    this.httpUser.deleteItem(body).subscribe({
      next: () => this.getAllUsers(),
      error: (err) => console.log(err)
    });
  }
}
