using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button _startBtn;

    void OnEnable()
    {
        _startBtn.onClick.AddListener(StartGame);
    }

    void OnDisable()
    {
        _startBtn.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        SceneHandler.Instance.ChangeScene();
    }
}
