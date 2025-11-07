using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryboardManager : MonoBehaviour
{
    [SerializeField] private List<SO_Frame> _framesInfosList;
    [SerializeField] private List<CutsceneContent> _contents;
    [SerializeField] private Button _nextFrameBtn;

    private int _currentContentIndex;
    private int _currentFrameIndex;
    void Awake()
    {
        _nextFrameBtn.gameObject.SetActive(true);
        _currentContentIndex = 0;
        _currentFrameIndex = 0;

        foreach (CutsceneContent content in _contents)
        {
            List<SO_Frame> framesToPopulate = new List<SO_Frame>();

            for (int i = 0; i < content.FramesQuantity; i++)
            {
                framesToPopulate.Add(_framesInfosList[_currentFrameIndex]);
                _currentFrameIndex++;
            }

            content.PopulateFrames(framesToPopulate);
        }

        ShowButtonFrame(GetContent());
    }

    void OnEnable() => _nextFrameBtn.onClick.AddListener(NextFrame);
    void OnDisable() => _nextFrameBtn.onClick.RemoveListener(NextFrame);
    private void NextFrame() => ShowButtonFrame(GetContent());

    private void ShowButtonFrame(CutsceneContent currentContent)
    {
        if (currentContent == null)
        {
            EndCutscene(true);
            return;
        }

        if (currentContent.HaveFrameToShow)
        {
            currentContent.ShowFrame();
            return;
        }

        currentContent.gameObject.SetActive(false);
        _currentContentIndex++;

        if (_currentContentIndex < _contents.Count)
        {
            CutsceneContent next = _contents[_currentContentIndex];
            next.gameObject.SetActive(true);
            ShowButtonFrame(next);
        }
        else
            EndCutscene(currentContent.EndFrame);
    }

    private void EndCutscene(bool isEndFrame)
    {
        if (isEndFrame)
            EventManager.StoryBoardIsEnded();
        else
            gameObject.SetActive(false);

    }

    private CutsceneContent GetContent()
    {
        if (_currentContentIndex < _contents.Count)
            return _contents[_currentContentIndex];
        return null;
    }
}
