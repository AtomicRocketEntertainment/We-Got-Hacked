
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName =" Email Info", menuName ="Scriptable Objcts/Objects Info/Email Info")]
public class SO_Email : ScriptableObject
{
    public string Index;
    public EmailType Type;
    public EmailSender Sender;
    public EventDispatcher DispatcherInfo;
    public bool HasResponse;
   
    [HorizontalLine(color: EColor.Blue), ResizableTextArea] public string Title;
    [ResizableTextArea] public string Content; 

    [BoxGroup("Response Area"), ShowIf(nameof(HasResponse)), ResizableTextArea] public string QuestionText;
    [BoxGroup("Response Area"), ShowIf(nameof(HasResponse))] public List<EmailResponse> Responses;
    [NonSerialized] public bool IsAnswered;
}

public enum EmailType
{
    SPAM, NEWS, LORE, HACKING
}

[Serializable]
public class EventDispatcher
{
    public bool HasEmailEvent;
    public bool HasNormalEvent;
    public EmailType EmailTypeToCreate;
    [HideInInspector] public bool EmailEventSended = false;
    [HideInInspector] public bool NormalEventSended = false;
}

[Serializable]
public struct EmailSender
{
    public Sprite Profile;
    public string Name;
    public string Email;
}

[Serializable]
public struct EmailResponse
{
    [ResizableTextArea] public string TextOption;
    [ResizableTextArea] public string EmailText;
    public bool IsCorrectAnswer;
}
