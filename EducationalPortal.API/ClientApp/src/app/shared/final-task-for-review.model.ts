import { ReviewQuestion } from "./review-question.model";

export class FinalTaskForReview {
    submittedFinalTaskId: number = 0;
    fileLink: string = "";
    finalTaskText: string = "";
    reviewQuestions: ReviewQuestion[] = [];
}
