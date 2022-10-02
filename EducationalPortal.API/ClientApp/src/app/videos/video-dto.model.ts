import { MaterialBase } from "../shared/material-base.model";
import { Quality } from "../shared/quality.model";

export class VideoDto extends MaterialBase {
    duration: number = 0;
    quality: Quality = new Quality();
    file: File;
}
