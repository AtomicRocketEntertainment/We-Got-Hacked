using UnityEngine;

public class OpenScreenHelper : MonoBehaviour
{
    [SerializeField] private GameObject _screenToOpen;
    [SerializeField] private bool _openWhenActive;

    private void OnEnable()
    {
        if (_openWhenActive)
            _screenToOpen.gameObject.SetActive(true);
    }

    public void OpenScreen()
    {
        _screenToOpen.SetActive(true);
    }
}
