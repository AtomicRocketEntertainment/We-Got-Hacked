using UnityEngine;
using UnityEngine.UI;

public class TalkingCutscene : MonoBehaviour
{
    [SerializeField] private Button _endStoryBoard;
    [SerializeField] private bool _isEndCutscene = true;

    void OnEnable() => _endStoryBoard.onClick.AddListener(EndStory);
    void OnDisable() => _endStoryBoard.onClick.RemoveListener(EndStory);
    private void EndStory()
    {
        if (_isEndCutscene)
            EventManager.StoryBoardIsEnded();
        else
            this.gameObject.SetActive(false);
    }
}
