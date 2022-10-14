import { UserDTO } from "../account/user-dto.model";

export class SubmittedFinalTask {
    fileLink: string = "";
    finalTaskId: number = 0;
    submitDateUTC: Date = new Date();
    mark: number = 0;
    reviewedBy: UserDTO;
    reviewedTask: boolean = false;
}
