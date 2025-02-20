
using UnityEngine;

[CreateAssetMenu(fileName =" Email Info", menuName ="Scriptable Objcts/Objects Info/Email Info")]
public class SO_Email : ScriptableObject
{

    public EmailSender Sender;
    [TextArea(2, 3)]
    public string Title;
    [TextArea(4, 20)]
    public string Content;

}

[System.Serializable]
public struct EmailSender
{
    public Sprite Profile;
    public string Name;
    public string Email;
}
