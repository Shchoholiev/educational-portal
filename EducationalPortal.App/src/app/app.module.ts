import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { CoursesComponent } from './courses/courses/courses.component';
import { CourseComponent } from './courses/courses/course/course.component';
import { HttpClientModule } from '@angular/common/http';
import { PaginationComponent } from './pagination/pagination.component';
import { AppRoutingModule } from './app-routing.module';
import { CourseDetailsComponent } from './courses/course-details/course-details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationBarComponent,
    CoursesComponent,
    CourseComponent,
    PaginationComponent,
    CourseDetailsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
