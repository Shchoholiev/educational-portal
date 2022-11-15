import { DeadlineTypes } from "./deadline-types.model";

export class DeadlineUserStatistics {
    dateTimeUTC: Date = new Date;
    deadlineType: DeadlineTypes = DeadlineTypes.FinalTaskReview;
}
