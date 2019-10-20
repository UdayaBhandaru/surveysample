export class ProProfsUserQuiz {
  quizId: string;
  quizUniqueName: string;
  quizTitle: string;
  quizLink: string;
  encodedQuizLink: string;
  quizStatus: string;
  quizMarks: number;
  quizTotalMarks: number;
  correctAnswers: number;
  wrongAnswers: number;
  attemptDate?: Date;
  percentComplete?: number;
}
