import { Course } from "./course.model";
import { UsersSkills } from "./user-skills.model";

export class User {
    id: number = 0;
    name: string = "";
    position: string = "";
    avatar: string = "";
    email: string = "";
    balance: string = "";
    usersSkills: UsersSkills[] = [];
    createdCourses: Course[] = [];
}
