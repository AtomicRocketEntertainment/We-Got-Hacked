using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =" Stock Company", menuName ="Scriptable Objcts/Objects Info/Stock Company")]
public class SO_Stock : ScriptableObject
{
    public Sprite Icon;
    public string CompanyName;
    [Tooltip("Valores menores que 50 são reajustados.")] public List<float> Values;
    public Color Color;


    private void OnValidate()
    {
        ClampValues();
    }

    private void ClampValues()
    {
        for (int i = 0; i < Values.Count; i++)
        {
            if (Values[i] < 50)
            {
                Values[i] = 50;
            }
        }
    }
}
