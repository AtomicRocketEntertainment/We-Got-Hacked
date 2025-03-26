using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(fileName ="Software Info", menuName ="Scriptable Objcts/Objects Info/Software")]
public class SO_Software : ScriptableObject
{
    public GameObject Prefab;
    public SoftwareType Type;
    [ShowIf(nameof(IsSite))]public string Website;
    public Sprite Icon;

    private bool IsSite()
    {
        return Type == SoftwareType.Site;
    }
}

public enum SoftwareType
{
    Desktop, Site
}
