
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName =" Email Info", menuName ="Scriptable Objcts/Objects Info/Email Info")]
public class SO_Email : ScriptableObject
{

    public EmailSender Sender;
   
    [HorizontalLine(color: EColor.Blue)]

    [ResizableTextArea]
    public string Title;
    [ResizableTextArea]
    public string Content;

    [HorizontalLine(color: EColor.Blue)]
    
    public bool HasResponse;

    [BoxGroup("Response Area"), ShowIf("HasResponse")]
    [ResizableTextArea]
    public string QuestionText;
    [BoxGroup("Response Area"), ShowIf("HasResponse")]
    public List<EmailResponse> Responses;
    [NonSerialized] public bool IsAnswered;

}

[System.Serializable]
public struct EmailSender
{
    public Sprite Profile;
    public string Name;
    public string Email;
}

[System.Serializable]
public struct EmailResponse
{
    [ResizableTextArea]
    public string TextOption;
    [ResizableTextArea]
    public string EmailText;
    public bool IsCorrectAnswer;
}
