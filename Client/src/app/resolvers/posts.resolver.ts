import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Post } from '../model/post';
import { User } from '../model/user';
import { AlertifyService } from '../services/alertify.service';
import { PostsService } from '../services/posts.service';
import { UserService } from '../services/user.service';

@Injectable()
export class PostsResolver implements Resolve<Post[]> {
  constructor(
    private postService: PostsService,
    private router: Router,
    private alertifyService: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Post[]> {
    return this.postService.getPostsFromFriends().pipe(
      catchError((error) => {
        this.alertifyService.error('Wystąpił problem.');
        return of(null);
      })
    );
  }
}