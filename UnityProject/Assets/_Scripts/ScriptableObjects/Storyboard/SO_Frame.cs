using System;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName ="Frame Info", menuName ="Scriptable Objcts/Storyboard/Frame")]
public class SO_Frame : ScriptableObject
{
    public Sprite Sprite;
    public FrameTextInfo TextStatus;
    public bool IsEndFrame;
}

[Serializable]
public struct FrameTextInfo
{
    [ResizableTextArea] public string Text;
}
