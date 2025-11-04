using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryboardManager : MonoBehaviour
{
    [SerializeField] private List<SO_Frame> _framesInfosList;
    [SerializeField] private List<CutsceneContent> _contents;

    private int _currentContentIndex;
    private int _currentFrameIndex;
    void Awake()
    {
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

        StartCoroutine(ShowFrames(GetContent()));
    }

    private IEnumerator ShowFrames(CutsceneContent currentContent)
    {
        while (currentContent.HaveFrameToShow)
        {
            currentContent.ShowFrame();
            yield return new WaitForSeconds(6f);
        }

        _currentContentIndex++;

        if (_currentContentIndex < _contents.Count)
        {
            currentContent.gameObject.SetActive(false);
            StartCoroutine(ShowFrames(GetContent()));
        }
        else
            EndCutscene(currentContent.EndFrame);
    }   

    private void EndCutscene(bool isEndFrame)
    {
        if(isEndFrame)
            EventManager.StoryBoardIsEnded();
        else
            gameObject.SetActive(false);

    }

    private CutsceneContent GetContent()
    {
        CutsceneContent content = _contents[_currentContentIndex];
        
        if(!content.HaveFrameToShow)
        {
            _currentContentIndex++;
            content = _contents[_currentContentIndex];
        }

        return content;
    }
}
