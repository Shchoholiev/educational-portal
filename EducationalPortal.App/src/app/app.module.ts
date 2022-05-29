import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { CoursesComponent } from './courses/courses/courses.component';
import { CourseComponent } from './courses/courses/course/course.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PaginationComponent } from './pagination/pagination.component';
import { AppRoutingModule } from './app-routing.module';
import { CourseDetailsComponent } from './courses/course-details/course-details.component';
import { RegisterComponent } from './account/register/register.component';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './account/login/login.component';
import { ProfileComponent } from './account/profile/profile.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthInterceptor } from './auth-interceptor';
import { AuthorComponent } from './account/author/author.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart/shopping-cart.component';
import { MyLearningComponent } from './courses/my-learning/my-learning.component';
import { CourseProgressComponent } from './courses/my-learning/course-progress/course-progress.component';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    NavigationBarComponent,
    CoursesComponent,
    CourseComponent,
    PaginationComponent,
    CourseDetailsComponent,
    RegisterComponent,
    LoginComponent,
    ProfileComponent,
    AuthorComponent,
    ShoppingCartComponent,
    MyLearningComponent,
    CourseProgressComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7016"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
