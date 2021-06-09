import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Post } from '../model/post';

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getPostsFromFriends(): Observable<Post[]> {
    return this.http
      .get<Post[]>(this.baseUrl + 'posts', { observe: 'response' })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  getPostsForUser(userId: number): Observable<Post[]> {
    return this.http
      .get<Post[]>(this.baseUrl + 'posts/' + userId, { observe: 'response' })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  deletePost(id: number) {
    return this.http.delete(this.baseUrl + 'posts/' + id);
  }

  sendLike(id: number, like: any) {
    return this.http.post(this.baseUrl + 'posts/' + id + '/like', like);
  }

  getLikes(id: number): Observable<any[]> {
    return this.http
      .get<any[]>(this.baseUrl + 'posts/' + id + '/likes', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  getLike(id: number): Observable<any> {
    return this.http
      .get<any[]>(this.baseUrl + 'posts/' + id + '/like', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }
}
