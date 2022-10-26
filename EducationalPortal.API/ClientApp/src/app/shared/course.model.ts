import { FinalTask } from "./final-task.model";
import { MaterialBase } from "./material-base.model";
import { Skill } from "./skill.model";
import { User } from "./user.model";

export class Course {
    id: number = 0;
    name: string = "";
    thumbnail: string = "";
    shortDescription: string = "";
    description: string = "";
    price: number = 0;
    updateDateUTC: Date = new Date;
    studentsCount: number = 0;
    learningTime: number = 0;
    isBought: boolean = false;
    materials: MaterialBase[] = [];
    skills: Skill[] = [];
    author: User;
    finalTask: FinalTask;
}