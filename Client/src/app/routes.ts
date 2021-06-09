import { Routes } from '@angular/router';
import { EditComponent } from './edit/edit.component';
import { AnonymousGuard } from './guards/anonymous.guard';
import { AuthGuard } from './guards/auth.guard';
import { PreventUnsaved } from './guards/unsaved.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';
import { PostsResolver } from './resolvers/posts.resolver';
import { ProfileResolver } from './resolvers/profile.resolver';

export const appRoutes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AnonymousGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    canDeactivate: [PreventUnsaved],
    runGuardsAndResolvers: 'always',
    canActivate: [AnonymousGuard],
  },
  {
    path: 'home',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    resolve: { posts: PostsResolver },
    component: HomeComponent,
  },
  {
    path: 'users/:id',
    canActivate: [AuthGuard],
    resolve: { user: ProfileResolver },
    component: ProfileComponent,
  },
  {
    path: 'edit',
    canActivate: [AuthGuard],
    component: EditComponent
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' },
];
