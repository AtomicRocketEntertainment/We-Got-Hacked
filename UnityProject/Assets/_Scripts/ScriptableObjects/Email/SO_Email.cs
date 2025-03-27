
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

[CreateAssetMenu(fileName =" Email Info", menuName ="Scriptable Objcts/Objects Info/Email Info")]
public class SO_Email : ScriptableObject
{
    public EmailType Type;
    public EmailSender Sender;
   
    [HorizontalLine(color: EColor.Blue)]

    [ResizableTextArea]
    public string Title;
    [ResizableTextArea]
    public string Content;

    public EventDispatcher DispatcherInfo;

    [HorizontalLine(color: EColor.Blue)]
    
    public bool HasResponse;

    [BoxGroup("Response Area"), ShowIf("HasResponse")]
    [ResizableTextArea]
    public string QuestionText;
    [BoxGroup("Response Area"), ShowIf("HasResponse")]
    public List<EmailResponse> Responses;
    [NonSerialized] public bool IsAnswered;

}

public enum EmailType
{
    SPAM, NEWS, LORE, HACKING
}

[Serializable]
public struct EventDispatcher
{
    public bool HasEvent;
    public EmailType TypeToCreate;
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
    [ResizableTextArea]
    public string TextOption;
    [ResizableTextArea]
    public string EmailText;
    public bool IsCorrectAnswer;
}
