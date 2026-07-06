using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public static SceneHandler Instance { get; private set; }
    [BoxGroup("Initial Configs"), SerializeField] private SO_Scene _firstScene;
    [BoxGroup("Initial Configs"), SerializeField] private SO_Scene _menuScene;
    [BoxGroup("Initial Configs"), SerializeField] private SO_Scene _gameOverScene;
    [BoxGroup("Initial Configs"), SerializeField] private SO_Scene _alternativeEndingScene;
    [BoxGroup("Scenes Mapping"), SerializeField] private SO_Scene[] _allGameScenes;

    private SO_Scene _currentScene;
    private int _currentSceneIndex;
    private Dictionary<int, SO_Scene> _sceneMap;

    public int CurrentSceneIndex => _currentSceneIndex;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        PrepareSettings();
    }

    private void PrepareSettings()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        ResetCurrentSceneToFirst();

        _sceneMap = new Dictionary<int, SO_Scene>();
        int currentSceneIndex = 0;

        foreach (SO_Scene scene in _allGameScenes)
        {
            _sceneMap[currentSceneIndex] = scene;
            currentSceneIndex++;
        }
    }

    private void ResetCurrentSceneToFirst()
    {
        _currentScene = _firstScene;
        _currentSceneIndex = _currentScene.SceneIndex;
    }

    void OnEnable()
    {
        EventManager.OnEndStoryBoard += ChangeScene;
        EventManager.OnAlternativeEndingBoard += AlternativeEnding;
    }

    void OnDisable()
    {
        EventManager.OnEndStoryBoard -= ChangeScene;
        EventManager.OnAlternativeEndingBoard -= AlternativeEnding;
    }

    public void ChangeScene()
    {
        _currentSceneIndex = _currentScene.GoToScene.SceneIndex;
        _currentScene = _currentScene.GoToScene;

        SceneManager.LoadScene(_currentSceneIndex);
    }

    public void AlternativeEnding()
    {
        _currentSceneIndex = _alternativeEndingScene.SceneIndex;
        _currentScene = _alternativeEndingScene;

        SceneManager.LoadScene(_currentSceneIndex);
    }

    public void GoToGameOver()
    {
        _currentSceneIndex = _gameOverScene.SceneIndex;
        _currentScene = _gameOverScene;

        SceneManager.LoadScene(_currentSceneIndex);
    }

    public void StartGame()
    {
        //ResetCurrentSceneToFirst();
        SceneManager.LoadScene(_firstScene.GoToScene.SceneIndex);
    }
    public void ContinueGame()
    {
        _currentSceneIndex = _currentScene.SceneIndex;
        SceneManager.LoadScene(_currentSceneIndex);
    } 
    public void SetSceneByIndex(int sceneIndex) => _currentScene = sceneIndex == 0 ? _firstScene : _sceneMap[sceneIndex - 1]; //Minus one because game scene starts at index 1.
    public bool IsGameplayScene() => _currentScene.IsGamePlayScene;
}
