using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName ="Generic Response", menuName ="Scriptable Objcts/Response/Generic Response")]
public class SO_GenericResponse : ScriptableObject
{
    [BoxGroup("Response Area"), ResizableTextArea] public string Index;
    [BoxGroup("Response Area"), ResizableTextArea] public string QuestionText;
    [BoxGroup("Response Area"), ResizableTextArea] public string ConfirmQuestionText;
    [BoxGroup("Response Area"), ResizableTextArea] public string WrongFeedbackQuestionText;
    [BoxGroup("Response Area")] public List<GenericResponse> Responses;
}

[Serializable]
public class GenericResponse
{
    [ResizableTextArea] public string TextOption;
    [ResizableTextArea, ShowIf(nameof(HasTextToUpdate)), Tooltip("This is used only on war room's answers")] public string TextToUpdate; //used exclusive on war room
    public bool IsCorrectAnswer;
    public bool HasTextToUpdate;
}
