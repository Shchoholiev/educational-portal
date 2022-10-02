import { MaterialBase } from "./material-base.model";
import { Quality } from "./quality.model";

export class Video extends MaterialBase {
    duration: Date = new Date();
    quality: Quality = new Quality();
}