import { ReviewQuestion } from "./review-question.model";

export class FinalTask {
    id: number = 0;
    name: string = "";
    text: string = "";
    reviewDeadlineTime: Date = new Date();
    reviewQuestions: ReviewQuestion[] = [];
}
