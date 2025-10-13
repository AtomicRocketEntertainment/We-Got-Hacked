using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Console Info", menuName = "Scriptable Objcts/Objects Info/Console Info")]
public class SO_ConsoleInfos : ScriptableObject
{
    private const int _size = 5;
    [HideInInspector] public int Size => _size;

    [ResizableTextArea] public string[] Timestamp = new string[_size];
    [ResizableTextArea] public string[] Origin = new string[_size];
    [ResizableTextArea] public string[] Destiny = new string[_size];
    [ResizableTextArea] public string[] Protocol = new string[_size];
    [ResizableTextArea] public string[] Archive = new string[_size];
    public ConsoleContent[] ConsoleContent = new ConsoleContent[_size];
}

[System.Serializable]
public class ConsoleContent
{
    [ResizableTextArea] public string Name;
    [ResizableTextArea] public string DecryptedMessage;
    public bool IsCorrect;
}
