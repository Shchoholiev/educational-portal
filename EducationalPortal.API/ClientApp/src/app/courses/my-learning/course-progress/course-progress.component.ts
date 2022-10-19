import { Component, Input, OnInit } from '@angular/core';
import { CertificatesService } from 'src/app/certificates/certificates.service';
import { UsersCourses } from 'src/app/shared/users-courses.model';

@Component({
  selector: 'app-course-progress',
  templateUrl: './course-progress.component.html',
  styleUrls: ['./course-progress.component.css']
})
export class CourseProgressComponent implements OnInit {

  @Input() userCourse: UsersCourses = new UsersCourses();

  public progress: number = 0;

  constructor(private _certificateService: CertificatesService) { }

  ngOnInit(): void {
    if (this.userCourse.learnedMaterialsCount) {
      this.progress = Math.ceil(this.userCourse.learnedMaterialsCount * 100 / this.userCourse.materialsCount);
    }
  }

  public getCertificate(){
    this._certificateService.getCertificate(this.userCourse.course.id).subscribe(
      response => {
        var url = window.URL.createObjectURL(response);
        window.open(url);
      }
    )
  }
}
