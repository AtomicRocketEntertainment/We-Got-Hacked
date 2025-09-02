using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class MeetingPersonLines
{
    private int _currentLine = 0;
    public Line[] Lines;

    public MeetingPersonLines(Line[] lines)
    {
        this.Lines = lines;
        _currentLine = 0;
    }

    public Line GetCurrentLine()
    {
        Line currentLine = Lines[_currentLine];
        return currentLine;
    }

    public void AvanceLine() => _currentLine++;
}

[System.Serializable]
public struct Line
{
    [TextArea(5, 20)] public string Text;
    public bool IsLineToAnswer;
    public bool ShouldUpdateStream;
    [ShowIf(nameof(ShouldUpdateStream))] public Sprite IlustrationToShow;
}
