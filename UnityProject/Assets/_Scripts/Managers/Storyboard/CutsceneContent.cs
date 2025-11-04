using System.Collections.Generic;
using UnityEngine;

public class CutsceneContent : MonoBehaviour
{
    public List<CutsceneFrame> _frames;
    private int _currentFrameIndex = 0;
    public void PopulateFrames(List<SO_Frame> frames)
    {
        for(int i = 0; i < frames.Count; i++)
            _frames[i].SetFrame(frames[i].Sprite, frames[i].TextStatus.Text, frames[i].IsEndFrame);
    }

    public void ShowFrame()
    {
        _frames[_currentFrameIndex].ShowFrame();
        _currentFrameIndex++;
    }

    public int FramesQuantity => _frames.Count;
    public bool HaveFrameToShow => _currentFrameIndex != _frames.Count;
    public bool EndFrame => _frames[_currentFrameIndex - 1].IsLastFrame;
}
