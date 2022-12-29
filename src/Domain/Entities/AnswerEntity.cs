﻿namespace Domain.Entities;

public class AnswerEntity : BaseEntity
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public QuestionEntity? Question { get; set; }
}