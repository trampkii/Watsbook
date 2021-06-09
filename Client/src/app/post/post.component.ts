import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { runInThisContext } from 'vm';
import { Post } from '../model/post';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  @Input() post: Post;
  @Output() postDeleted = new EventEmitter<string>();
  likes: number;
  isLiked: boolean;

  constructor(
    private authService: AuthService,
    private postsService: PostsService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.getLikes();
  }

  isPostMine(): boolean {
    if (this.post.userId === this.authService.currentUser.id) {
      return true;
    } else {
      return false;
    }
  }

  deletePost() {
    this.alertify.confirm('Na pewno chcesz usunąć tego pościka mordo?', () => {
      this.postsService.deletePost(this.post.id).subscribe(
        (next) => {
          this.postDeleted.emit();
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    });
  }

  getLikes() {
    this.postsService.getLikes(this.post.id).subscribe(
      (res: any[]) => {
        this.likes = res.length;
        this.getLike();
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  sendLike() {
    const like = {
      userId: this.authService.currentUser.id,
      postId: this.post.id,
    };
    this.postsService.sendLike(this.post.id, like).subscribe(
      (next) => {
        this.getLikes();
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  getLike() {
    this.postsService.getLike(this.post.id).subscribe(
      (res: any) => {
        if (res === null) {
          this.isLiked = false;
        } else {
          this.isLiked = true;
        }
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }
}
