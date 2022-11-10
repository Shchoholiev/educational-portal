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
import { MyLearningComponent } from './courses/my-learning/my-learning.component';
import { CourseLearnComponent } from './courses/course-learn/course-learn.component';
import { CourseEditComponent } from './courses/course-edit/course-edit.component';
import { CourseCreateComponent } from './courses/course-create/course-create.component';
import { BasicLayoutComponent } from './basic-layout/basic-layout.component';
import { CoursesSearchComponent } from './courses/courses-search/courses-search.component';
import { MaterialsStatisticsComponent } from './statistics/materials-statistics/materials-statistics.component';
import { SalesStatisticsComponent } from './statistics/sales-statistics/sales-statistics.component';
import { CoursesStatisticsComponent } from './statistics/courses-statistics/courses-statistics.component';
import { UsersStatisticsComponent } from './statistics/users-statistics/users-statistics.component';

const routes: Routes = [
  { path: '', redirectTo: 'courses', pathMatch: 'full' },
  { path: '', 
    component: BasicLayoutComponent, 
    children: [
      { path: 'courses', component: CoursesComponent, pathMatch: 'full' },
      { path: 'courses/filtered', component: CoursesSearchComponent, pathMatch: 'full' },
      { path: 'courses/filtered/:filter', component: CoursesSearchComponent, pathMatch: 'full' },
      { path: 'courses/:id', component: CourseDetailsComponent, pathMatch: 'full' },
      { path: 'courses/edit/:id', component: CourseEditComponent, canActivate: [AuthGuard], pathMatch: 'full' },
      { path: 'create-course', component: CourseCreateComponent, canActivate: [AuthGuard], pathMatch: 'full' },
      { path: 'account/register', component: RegisterComponent },
      { path: 'account/login', component: LoginComponent },
      { path: 'account/profile', component: ProfileComponent, canActivate: [AuthGuard] },
      { path: 'account/author/:email', component: AuthorComponent },
      { path: 'shopping-cart', component: ShoppingCartComponent },
      { path: 'my-learning', component: MyLearningComponent, canActivate: [AuthGuard] },
      { path: 'statistics/materials', component: MaterialsStatisticsComponent, canActivate: [AuthGuard] },
      { path: 'statistics/sales', component: SalesStatisticsComponent, canActivate: [AuthGuard] },
      { path: 'statistics/courses', component: CoursesStatisticsComponent, canActivate: [AuthGuard] },
      { path: 'statistics/users', component: UsersStatisticsComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'courses/learn/:id', component: CourseLearnComponent, canActivate: [AuthGuard] },
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
