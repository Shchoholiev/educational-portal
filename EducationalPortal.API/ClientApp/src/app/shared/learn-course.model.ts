import { MaterialBase } from "./material-base.model";

export class LearnCourse {
    id: number = 0;
    name: string = "";
    progress: number = 0;
    materials: MaterialBase[] = [];
}
