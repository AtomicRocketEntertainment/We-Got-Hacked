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

            int nextIndex = shift ? _currentIndex - 1 : _currentIndex + 1;
            if (!_loopNavigation)
            {
                if (nextIndex < 0 || nextIndex >= _inputFields.Count) return;
            }

            if (nextIndex < 0) nextIndex = _inputFields.Count - 1;
            if (nextIndex >= _inputFields.Count) nextIndex = 0;

            _currentIndex = nextIndex;

            SelectField();
        }
    }

    private void SelectField()
    {
        _inputFields[_currentIndex].Select();
        _inputFields[_currentIndex].ActivateInputField();
    }
}
