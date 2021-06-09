import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsersToSearch(keyWord?: string): Observable<User[]> {
    const params = new HttpParams().set('keyWord', keyWord);
    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.http
      .get<User>(this.baseUrl + 'users/' + id, { observe: 'response' })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  sendFriendRequest(id: number) {
    return this.http.post(this.baseUrl + 'users/invite/' + id, {});
  }

  getFriendRequests(): Observable<any[]> {
    return this.http
      .get<any[]>(this.baseUrl + 'users/requests', { observe: 'response' })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  declineRequest(id: number) {
    return this.http.delete(this.baseUrl + 'users/invite/' + id + '/decline');
  }

  acceptRequest(id: number) {
    return this.http.post(this.baseUrl + 'users/invite/' + id + '/accept', {});
  }

  getFriends(id: number): Observable<any[]> {
    return this.http
      .get<any[]>(this.baseUrl + 'users/' + id + '/friends', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  getFriendRelation(id: number): Observable<any> {
    return this.http
      .get<any>(this.baseUrl + 'users/relation/' + id, { observe: 'response' })
      .pipe(
        map((response) => {
          const res = response.body;
          return res;
        })
      );
  }

  deleteFriend(id: number) {
    return this.http.delete(this.baseUrl + 'users/friends/' + id);
  }
}
