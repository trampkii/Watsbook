import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from '../model/user';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
})
export class NavBarComponent implements OnInit {
  modalRef: BsModalRef;
  keyWord: string;
  searchedUsers: User[];
  friendRequests: any[];
  photoUrl: string;

  constructor(
    public authService: AuthService,
    private router: Router,
    private modalService: BsModalService,
    private userService: UserService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    if (this.authService.loggedIn()) {
      this.userService.getUser(this.authService.currentUser.id).subscribe(
        (user: User) => {
          this.photoUrl = user.photoUrl;
          this.authService.changeMemberPhoto(this.photoUrl);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    }
    this.authService.currentPhotoUrl.subscribe(
      (photoUrl) => (this.photoUrl = photoUrl)
    );
    this.keyWord = '';
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.router.navigate(['/login']);
  }

  loadUsers() {
    this.userService.getUsersToSearch(this.keyWord).subscribe(
      (res: User[]) => {
        this.searchedUsers = res;
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  onKey(event) {
    this.keyWord = event.target.value;
  }

  openSearchedUsers(template: TemplateRef<any>) {
    if (this.keyWord.length === 0) {
      this.alertify.message('Wprowadź słowo kluczowe');
      return;
    }
    this.modalRef = this.modalService.show(template);
    this.loadUsers();
  }

  loadFriendRequests(template: TemplateRef<any>) {
    this.userService.getFriendRequests().subscribe((res: any[]) => {
      this.friendRequests = res;
      this.modalRef = this.modalService.show(template);
    });
  }

  declineRequest(id: number) {
    this.alertify.confirm(
      'Jesteś pewien, że chcesz usunąć to zaproszenie?',
      () => {
        this.userService.declineRequest(id).subscribe(() => {
          this.alertify.message('Usunięto');
          this.userService.getFriendRequests().subscribe((res: any[]) => {
            this.friendRequests = res;
          });
        });
      }
    );
  }

  acceptRequest(id: number) {
    this.userService.acceptRequest(id).subscribe((data) => {
      this.alertify.success('Brawo! Jesteście przyjaciółmi.');
      this.userService.getFriendRequests().subscribe(
        (res: any[]) => {
          this.friendRequests = res;
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    });
  }
}
