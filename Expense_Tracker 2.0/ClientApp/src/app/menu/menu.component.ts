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

  ngOnInit(): void {
    this.user.currentMessage.subscribe(message => this.loggedIn = message);
    this.user.checkToken();
  }

}
