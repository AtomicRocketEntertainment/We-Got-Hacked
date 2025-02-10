using UnityEngine;

[CreateAssetMenu(fileName =" Email Info", menuName ="Scriptable Objcts/Objects Info/Email Info")]
public class SO_Email : ScriptableObject
{

    [TextArea(2, 3)]
    public string Sender;
    [TextArea(2, 3)]
    public string Title;
    [TextArea(4, 20)]
    public string Content;

}
