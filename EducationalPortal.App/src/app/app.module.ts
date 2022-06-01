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
import { CourseLearnComponent } from './courses/course-learn/course-learn.component';
import { ProgressBarComponent } from './courses/course-learn/progress-bar/progress-bar.component';
import { VideoSideComponent } from './courses/course-learn/materials/video-side/video-side.component';
import { BookSideComponent } from './courses/course-learn/materials/book-side/book-side.component';
import { ArticleSideComponent } from './courses/course-learn/materials/article-side/article-side.component';
import { BookLearnComponent } from './courses/course-learn/materials/book-learn/book-learn.component';
import { SafePipe } from './safe.pipe';
import { VideoLearnComponent } from './courses/course-learn/materials/video-learn/video-learn.component';
import { CourseEditComponent } from './courses/course-edit/course-edit.component';
import { AddSkillsComponent } from './skills/add-skills/add-skills.component';
import { SkillComponent } from './skills/skill/skill.component';
import { MaterialComponent } from './courses/course-edit/material/material.component';
import { AddArticleComponent } from './articles/add-article/add-article.component';
import { AddResourceComponent } from './resources/add-resource/add-resource.component';
import { AddBooksComponent } from './books/add-books/add-books.component';
import { CreateBookComponent } from './books/create-book/create-book.component';
import { AddAuthorsComponent } from './authors/add-authors/add-authors.component';
import { CreateAuthorComponent } from './authors/create-author/create-author.component';
import { AddVideosComponent } from './videos/add-videos/add-videos.component';
import { CreateVideoComponent } from './videos/create-video/create-video.component';

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
    CourseProgressComponent,
    CourseLearnComponent,
    ProgressBarComponent,
    VideoSideComponent,
    BookSideComponent,
    ArticleSideComponent,
    BookLearnComponent,
    SafePipe,
    VideoLearnComponent,
    CourseEditComponent,
    AddSkillsComponent,
    SkillComponent,
    MaterialComponent,
    AddArticleComponent,
    AddResourceComponent,
    AddBooksComponent,
    CreateBookComponent,
    AddAuthorsComponent,
    CreateAuthorComponent,
    AddVideosComponent,
    CreateVideoComponent
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
