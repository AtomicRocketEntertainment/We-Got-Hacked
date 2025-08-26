[System.Serializable]
public class MeetingPersonLines
{
    private int _currentLine = 0;
    public Line[] lines;

    public Line GetCurrentLine()
    {
        Line currentLine = lines[_currentLine++];
        return currentLine;
    }
}

[System.Serializable]
public struct Line
{
    public string Text;
    public bool IsLineToAnswer;
    public bool ShouldUpdateStream;
}
