using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Document Button List", menuName = "Scriptable Objcts/Remotopia/Document Button List")]
public class SO_DocumentButtonList : ScriptableObject
{
    public List<SO_DocumentButton> Buttons = new List<SO_DocumentButton>();
}
