
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName =" Email Info", menuName ="Scriptable Objcts/Objects Info/Email Info")]
public class SO_Email : ScriptableObject
{
    [BoxGroup("Common Infos")] public string Index;
    [BoxGroup("Common Infos")] public bool StartOpen;
    [BoxGroup("Common Infos")] public EmailType Type;
    [BoxGroup("Common Infos")] public EmailSender Sender;
    [BoxGroup("Common Infos"), ResizableTextArea] public string Title;
    [BoxGroup("Common Infos"), ResizableTextArea] public string Content; 
    [BoxGroup("Response Area")] public bool HasResponse;
    [BoxGroup("Response Area"), ShowIf(nameof(HasResponse)), ResizableTextArea] public string QuestionText;
    [BoxGroup("Response Area"), ShowIf(nameof(HasResponse)), ResizableTextArea] public string ConfirmQuestionText;
    [BoxGroup("Response Area"), ShowIf(nameof(HasResponse)), ResizableTextArea] public string WrongFeedbackQuestionText;
    [BoxGroup("Response Area"), ShowIf(nameof(HasResponse))] public List<EmailResponse> Responses;
    [BoxGroup("Event Info")] public bool HasEventToSend; 
    [BoxGroup("Event Info"), ShowIf(nameof(HasEventToSend))] public EventDispatcher DispatcherInfo;
    
    [NonSerialized] public bool IsAnswered;
}

public enum EmailType
{
    SPAM, NEWS, LORE, HACKING
}

[Serializable]
public class EventDispatcher
{
    public EventDispatcher() 
    { 
        EmailEventSended = false;
        NormalEventSended = false;
    }
    
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
