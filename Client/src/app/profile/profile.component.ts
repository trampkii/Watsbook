import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  TemplateRef,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Post } from '../model/post';
import { User } from '../model/user';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { PostsService } from '../services/posts.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  user: User;
  friends: any[];
  posts: Post[];
  modalRef: BsModalRef;
  isFriendOfMineSwitch: boolean;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private modalService: BsModalService,
    private postService: PostsService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.user = data['user'];
      this.loadPosts();
      this.isFriendOfMine();
    });
  }

  loadFriends(template: TemplateRef<any>) {
    this.userService.getFriends(this.user.id).subscribe(
      (res: User[]) => {
        this.friends = res;
        this.modalRef = this.modalService.show(template);
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  loadPosts() {
    this.postService.getPostsForUser(this.user.id).subscribe(
      (res: Post[]) => {
        this.posts = res;
      },
      (error) => this.alertify.error(error)
    );
  }

  isFriendOfMine() {
    this.userService.getFriendRelation(this.user.id).subscribe(
      (res: any) => {
        if (res === null) {
          this.isFriendOfMineSwitch = false;
        } else {
          this.isFriendOfMineSwitch = true;
        }
      },
      (error) => {
        this.isFriendOfMineSwitch = false;
      }
    );
  }

  isMyAccount(): boolean {
    if (this.user.id === this.authService.currentUser.id) {
      return true;
    } else {
      return false;
    }
  }

  sendInvitation() {
    this.userService.sendFriendRequest(this.user.id).subscribe(
      (res) => {
        this.alertify.message('Wysłano zaproszenie.');
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  deleteFriend() {
    this.alertify.confirm(
      'Czy na pewno chcesz zakończyć tę cudowną znajomość?',
      () => {
        this.userService.deleteFriend(this.user.id).subscribe(
          (next) => {
            this.isFriendOfMine();
          },
          (error) => {
            this.alertify.error(error);
          }
        );
      }
    );
  }
}
