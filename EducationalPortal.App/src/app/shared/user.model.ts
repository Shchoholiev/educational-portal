import { Course } from "./course.model";

export class User {
    id: number = 0;
    name: string = "";
    position: string = "";
    avatar: string = "";
    email: string = "";
    balance: string = "";
    createdCourses: Course[] = [];
}
