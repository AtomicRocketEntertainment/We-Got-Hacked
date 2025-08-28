using UnityEngine;

public interface IMeetingPersonInstance
{
    void UpdateMyCard(Sprite profile);
    void OpenMicAndTalk(string text);
    void CloseMic();
}
