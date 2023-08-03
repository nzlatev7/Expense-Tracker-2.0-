import { Component, OnInit } from '@angular/core';
import { UserService } from '../core/services/user.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  message: string | undefined;

  constructor(private user: UserService) { }

  loggedIn = false;
  isAdmin = false;

  ngOnInit(): void {
    this.user.logged.subscribe(message => this.loggedIn = message);
    this.user.Admin.subscribe(message => this.isAdmin = message);
    this.user.checkToken();
  }

}
