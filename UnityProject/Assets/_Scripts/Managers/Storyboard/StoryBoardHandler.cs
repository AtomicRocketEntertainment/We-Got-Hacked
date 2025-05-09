using UnityEngine;

public class StoryBoardHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] _sceneStoryBoards;
    [SerializeField] private bool _shouldStartWithStoryBoard;
    private int _currentStoryBoard = 0;

    void Awake()
    {
        if(_shouldStartWithStoryBoard) ShowStoryBoard();
    }

    private void OnEnable() {
        
        EventManager.OnStoryBoardNeeded += ShowStoryBoard;
    }

    void OnDisable()
    {
        EventManager.OnStoryBoardNeeded -= ShowStoryBoard;
    }

    private void ShowStoryBoard()
    {
        _sceneStoryBoards[_currentStoryBoard].SetActive(true);
        _currentStoryBoard++;
    }
}
