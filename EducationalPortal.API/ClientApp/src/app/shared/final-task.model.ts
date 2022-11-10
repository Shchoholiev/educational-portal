import { ReviewQuestion } from "./review-question.model";

export class FinalTask {
    id: number = 0;
    name: string = "";
    text: string = "";
    reviewDeadlineTime: Date = new Date('0001-01-01T00:00:00Z');
    reviewQuestions: ReviewQuestion[] = [];
}
