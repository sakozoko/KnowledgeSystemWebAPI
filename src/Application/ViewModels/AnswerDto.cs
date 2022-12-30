﻿namespace Application.ViewModels;

public class AnswerDto : BaseViewModel
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}