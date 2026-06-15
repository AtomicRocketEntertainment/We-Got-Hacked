using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class MiniClassroomUI : MonoBehaviour
    {
        [SerializeField] private MiniClassRoomManager _manager;
        [SerializeField] private DialogueInput _dialogueInput;
        [SerializeField] private LogPanel _logUI;
        [SerializeField] private List<ActorUI> _actorsUI;
        [SerializeField] private Image _slideImg;
        [SerializeField] private TextBoxUI _textBox;
        [SerializeField] private RectTransform _skipTopicPanel;
        [SerializeField] private RectTransform _skipClassPanel;
        [SerializeField] private Button _screenButton;
        [SerializeField] private Button _autoRunButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _logButton;

        [SerializeField] private Button _confirmTopicButton;
        [SerializeField] private Button _cancelTopicButton;

        [SerializeField] private Button _skipClassButton;
        [SerializeField] private Button _confirmSkipClassButton;
        [SerializeField] private Button _cancelSkipClassButton;

        private DialogueLine _currentLine;
        private ActorSO _currentActorHighlighted;

        private int _pendingAnimations = 0;

        private void Start()
        {
            _screenButton.onClick.AddListener(_dialogueInput.OnNextClick);
            _autoRunButton.onClick.AddListener(ToggleAutoRun);
            _stopButton.onClick.AddListener(ToggleAutoRun);
            _logButton.onClick.AddListener(OpenLogHistory);

            _confirmTopicButton.onClick.AddListener(_manager.SkipTopic);
            _cancelTopicButton.onClick.AddListener(CancelSkipTopic);

            _skipClassButton.onClick.AddListener(ShowSkipClassPanel);
            _cancelSkipClassButton.onClick.AddListener(HideSkipClassPanel);
            _confirmSkipClassButton.onClick.AddListener(_manager.FinishScene);
        }

        private void OnDisable()
        {
            _screenButton.onClick.RemoveAllListeners();
            _autoRunButton.onClick.RemoveAllListeners();
            _stopButton.onClick.RemoveAllListeners();
            _logButton.onClick.RemoveAllListeners();

            _confirmSkipClassButton.onClick.RemoveAllListeners();
            _cancelTopicButton.onClick.RemoveAllListeners();

            _skipClassButton.onClick.RemoveAllListeners();
            _cancelSkipClassButton.onClick.RemoveAllListeners();
            _confirmSkipClassButton.onClick.RemoveAllListeners();
        }

        private void CheckAnimationsComplete(Action onComplete)
        {
            _pendingAnimations--;

            if (_pendingAnimations <= 0)
            {
                onComplete?.Invoke();
            }
        }

        private void OnActorAnimationsComplete()
        {
            if(_manager.State != DialogueState.ActorsAnimating) return;
            _manager.SwitchState(DialogueState.WritingDialog);
        }

        private void SetBackground()
        {
            if (_currentLine.slide == null) return;
            if (_slideImg.sprite == _currentLine.slide) return;
            _slideImg.sprite = _currentLine.slide;
        }

        private void ToggleAutoRun()
        {
            _manager.ToggleAutoMode();
            ToggleAutoRunButton();
        }

        private void CancelSkipTopic()
        {
            HideSkipTopicPanel();
            _manager.Next();
        }

        private void ShowSkipClassPanel()
        {
            _skipClassPanel.gameObject.SetActive(true);
        }

        private void HideSkipClassPanel()
        {
            _skipClassPanel.gameObject.SetActive(false);
        }

        public void ToggleAutoRunButton()
        {
            _stopButton.gameObject.SetActive(_manager.AutoMode);
            _autoRunButton.gameObject.SetActive(!_manager.AutoMode);
        }

        public void OpenLogHistory()
        {
            List<DialogueLine> lines = _manager.History;
            _logUI.OpenLog(lines);
        }

        public void ShowSkipTopicPanel()
        {
            _skipTopicPanel.gameObject.SetActive(true);
        }

        public void HideSkipTopicPanel()
        {
            _skipTopicPanel.gameObject.SetActive(false);
        }

        public void SetConversation(DialogueLine line)
        {
            _currentLine = line;

            _currentActorHighlighted = _currentLine.GetHighlightedActor();

            SetBackground();

            _manager.SwitchState(DialogueState.ActorsAnimating);
        }

        public void SetDialogue()
        {
            if (_currentActorHighlighted != null && _currentActorHighlighted.name != "")
                _textBox.SetActorName(_currentActorHighlighted);
            else
                _textBox.SetActorName();

            _textBox.SetDialogue(_currentLine.dialogueText);
        }

        public void SetActors()
        {
            _pendingAnimations = 0;

            if (_currentLine.actors != null && _currentLine.actors.Count > 0)
            {
                int actorCount = 0;

                foreach (ActorUI actor in _actorsUI)
                {
                    if (actorCount >= _currentLine.actors.Count)
                    {
                        _pendingAnimations++;

                        actor.RemoveActor(() =>
                        {
                            CheckAnimationsComplete(OnActorAnimationsComplete);
                        });
                        continue;
                    }

                    if (actor.ActorID == null || actor.ActorID == "")
                    {
                        _pendingAnimations++;

                        actor.OnAnimationComplete = () =>
                        {
                            CheckAnimationsComplete(OnActorAnimationsComplete);
                        };
                    }

                    actor.SetActor(_currentLine.actors[actorCount].actor, _currentLine.actors[actorCount].headID, _currentLine.actors[actorCount].bodyID);
                    if (_currentLine.actorHighlightedID != null && _currentLine.actorHighlightedID != "")
                    {
                        bool isHighlighted = _currentLine.actors[actorCount].actor.id == _currentLine.actorHighlightedID;
                        actor.SetHighlight(isHighlighted);
                    }
                    else
                        actor.SetNormalActor();
                    actorCount++;
                }
            }

            if (_pendingAnimations == 0)
            {
                OnActorAnimationsComplete();
            }
        }

        public void SkipActorsAnim(bool proceed = true)
        {
            foreach (ActorUI actor in _actorsUI)
            {
                actor.SkipAnimation();
            }
            if(proceed) OnActorAnimationsComplete();
        }

        public void SkipDialogue(bool proceed = true)
        {
             _textBox.SkipText();
            if(proceed) DialogueTextReady();
        }

        public void DialogueTextReady()
        {
            if (_manager.State != DialogueState.WritingDialog) return;
            _manager.SwitchState(DialogueState.DialogFinished);
        }

        public void ClearConversation()
        {
            _textBox.ClearDialogue();
            foreach(ActorUI actorUI in _actorsUI)
            {
                actorUI.ClearActor();
            }
        }
    }
}
