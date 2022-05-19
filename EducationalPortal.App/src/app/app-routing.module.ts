import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CoursesComponent } from './courses/courses/courses.component';
import { CourseDetailsComponent } from './courses/course-details/course-details.component';
import { RegisterComponent } from './account/register/register.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './account/login/login.component';
import { ProfileComponent } from './account/profile/profile.component';
import { AuthorComponent } from './account/author/author.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart/shopping-cart.component';

const routes: Routes = [
  { path: 'courses', component: CoursesComponent },
  { path: 'courses/:id', component: CourseDetailsComponent },
  { path: 'account/register', component: RegisterComponent },
  { path: 'account/login', component: LoginComponent },
  { path: 'account/profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'account/author/:email', component: AuthorComponent },
  { path: 'shopping-cart', component: ShoppingCartComponent },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes),
    CommonModule
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
