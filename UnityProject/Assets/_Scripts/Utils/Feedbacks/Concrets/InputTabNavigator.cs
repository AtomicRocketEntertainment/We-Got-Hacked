using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTabNavigator : MonoBehaviour
{
    [SerializeField] private List<TMP_InputField> _inputFields;
    [SerializeField] private bool _loopNavigation;

    private int _currentIndex = 0;


    private void OnEnable()
    {
        _currentIndex = 0;
        SelectField();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            GameObject current = EventSystem.current.currentSelectedGameObject;

            for (int i = 0; i < _inputFields.Count; i++)
            {
                if (_inputFields[i].gameObject == current)
                {
                    int nextIndex = shift ? i - 1 : i + 1;

                    if (_loopNavigation)
                    {
                        if (nextIndex < 0) nextIndex = _inputFields.Count - 1;
                        if (nextIndex >= _inputFields.Count) nextIndex = 0;
                    }
                    else
                    {
                        if (nextIndex < 0) nextIndex = 0;
                        if (nextIndex >= _inputFields.Count) nextIndex = _inputFields.Count - 1;
                    }
                    _currentIndex = nextIndex;

                    SelectField();
                    break;
                }
            }
        }
    }

    private void SelectField()
    {
        _inputFields[_currentIndex].Select();
        _inputFields[_currentIndex].ActivateInputField();
    }
}
