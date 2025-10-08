using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;


public class ButtonGroupFeedbackManager : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [BoxGroup("First Active Feedback"), SerializeField] private bool _firstButtonActive = false;
    [BoxGroup("First Active Feedback"), SerializeField, ShowIf(nameof(_firstButtonActive))] private FeedbackButton _firstButton;

    void Awake()
    {
        DesactiveFeedbacks();
        
        if (_firstButtonActive)
            _firstButton.ActiveFeedback();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            int index = i;
            if (_buttons[index].TryGetComponent(out IFeedbackGroupButton fb))
                _buttons[index].onClick.AddListener(() => ActiveButton(fb));
        }
    }

    private void ActiveButton(IFeedbackGroupButton button)
    {
        DesactiveFeedbacks();
        button.ActiveFeedback();
    }

    private void DesactiveFeedbacks()
    {
        foreach (Button button in _buttons)
        {
            if (button.TryGetComponent(out IFeedbackGroupButton fb))
                fb.DesactiveFeedback();
        }
    }
}
