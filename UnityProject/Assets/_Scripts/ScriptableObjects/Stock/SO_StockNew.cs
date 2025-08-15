using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "Stock News", menuName ="Scriptable Objcts/Objects Info/Stock News")]
public class SO_StockNew : ScriptableObject
{
    [TextArea(4, 20)]public string Header;
    [TextArea(4, 20)]public string Content;
}
