using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Document Button", menuName = "Scriptable Objcts/Remotopia/Document Button")]
public class SO_DocumentButton : ScriptableObject
{
    public int Index;
    public Sprite Icon;
    public string Name;
    public DocType Type;
    [ShowIf("IsFolder")] public List<SO_DocumentButton> FolderButtons = new List<SO_DocumentButton>();
    [ShowIf("IsTxt")] public string Text;


    public bool IsFolder() => Type == DocType.FOLDER;
    public bool IsTxt() => Type == DocType.TXT;

    [ContextMenu("GenerateRandomIndex")]
    public void GenerateRandomIndex() => Index = Random.Range(1, int.MaxValue);
}

public enum DocType
{
    FOLDER, TXT, BLOCKED
}
