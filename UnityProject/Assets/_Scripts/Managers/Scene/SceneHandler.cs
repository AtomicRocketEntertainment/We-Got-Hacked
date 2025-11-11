using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public static SceneHandler Instance { get; private set;}
    [SerializeField] private SO_Scene _firstScene; 
    [SerializeField] private SO_Scene _menuScene; 
    private SO_Scene _currentScene;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log($"There can be only one instance of {name}");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        _currentScene = _firstScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
            ChangeScene();
    }

    void OnEnable()
    {
        EventManager.OnEndStoryBoard += ChangeScene;
    }

    void OnDisable()
    {
        EventManager.OnEndStoryBoard -= ChangeScene;
    }

    public void ChangeScene()
    {
        int goToIndex = _currentScene.GoToScene.SceneIndex;
        _currentScene = _currentScene.GoToScene;

        SceneManager.LoadScene(goToIndex);
    }

}
