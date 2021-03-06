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
    materials: MaterialBase[] = [];
    skills: Skill[] = [];
    author: User;
}