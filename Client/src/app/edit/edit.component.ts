import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { User } from '../model/user';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent implements OnInit {
  uploader: FileUploader;
  baseUrl = environment.apiUrl;
  photoUrl: string;
  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(
      (photoUrl) => (this.photoUrl = photoUrl)
    );
    this.initializeUploader();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onCompleteAll = () => {
      this.alertify.success('dodano.');
      this.userService.getUser(this.authService.currentUser.id).subscribe(
        (user: User) => {
          this.photoUrl = user.photoUrl;
          this.authService.changeMemberPhoto(this.photoUrl);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
      this.uploader.clearQueue();
    };
  }

  uploadPost() {
    if (this.uploader.getNotUploadedItems().length) {
      this.uploader.uploadAll();
    } else {
      this.alertify.message('dodaj zdjęcie!');
    }
  }
}
