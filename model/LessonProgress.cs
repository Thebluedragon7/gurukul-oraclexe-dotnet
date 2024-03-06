namespace gurukul.model;

public enum LessonStatusEnum
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}


public class LessonProgress
{
    public string LessonProgressId;
    public string LessonId;
    public string EnrolmentId;
    public LessonStatusEnum LessonStatus;
}
