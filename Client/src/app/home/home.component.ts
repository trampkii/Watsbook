import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { userInfo } from 'os';
import { environment } from 'src/environments/environment';
import { Post } from '../model/post';
import { User } from '../model/user';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { PostsService } from '../services/posts.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  friendPosts: Post[];
  uploader: FileUploader;
  baseUrl = environment.apiUrl;
  postContent = '';
  photoUrl: string;

  constructor(
    private route: ActivatedRoute,
    public authService: AuthService,
    private alertify: AlertifyService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.friendPosts = data['posts'];
      this.userService.getUser(this.authService.currentUser.id).subscribe(
        (user: User) => {
          this.photoUrl = user.photoUrl;
          this.authService.changeMemberPhoto(this.photoUrl);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
      this.initializeUploader();
      this.initializeUploader();
    });
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'posts',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      additionalParameter: { content: this.postContent },
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onBeforeUploadItem = (file) => {
      this.uploader.options.additionalParameter = {
        content: this.postContent,
      };
    };

    this.uploader.onCompleteAll = () => {
      this.alertify.success('dodano.');
      this.uploader.clearQueue();
    };
  }

  onKey(event) {
    if (event.key === 'Enter') {
      this.uploadPost();
    } else {
      this.postContent = event.target.value;
    }
  }

  uploadPost() {
    if (this.uploader.getNotUploadedItems().length) {
      this.uploader.uploadAll();
    } else {
      this.alertify.message('dodaj zdjęcie!');
    }
  }
}
