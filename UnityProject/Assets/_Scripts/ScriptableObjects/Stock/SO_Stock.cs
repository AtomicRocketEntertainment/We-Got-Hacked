using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =" Stock Company", menuName ="Scriptable Objcts/Objects Info/Stock Company")]
public class SO_Stock : ScriptableObject
{
    public Sprite Icon;
    public string CompanyName;
    public List<float> Values;
    public Color Color;
}
