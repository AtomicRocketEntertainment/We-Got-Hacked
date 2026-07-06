using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    public class MiniClassRoomManager : MonoBehaviour
    {
        //[SerializeField] private ConversationSO _sequence;
        [SerializeField] private MiniClassroomUI _miniClassroomUI;
        [SerializeField] private DialogueInput _dialogueInput;
        [SerializeField] private DialogueLoaderSO _loader;
        //SecretEnding
        [SerializeField] private SecretEndingController _secretEndingController;
        [SerializeField] private int _secretEndingSlide = 0;

        private List<DialogueLine> _sequence;

        private DialogueState _currentState;
        private DialogueLine _currentLine;

        private int _currentIndex = 0;
        private List<DialogueLine> _history = new();

        private bool _autoMode = false;
        private bool _isTopicJump = false;
        private Coroutine _autoCoroutine;

        private bool _alternativeEnding = false;

        public DialogueState State => _currentState;
        public bool AutoMode => _autoMode;
        public List<DialogueLine> History => _history;

        private void Start()
        {
            _sequence = _loader.LoadDialogue();
            StartConversation();
        }

        private void StartDialogue()
        {
            _currentLine = _sequence[_currentIndex];
            _history.Add(_currentLine);

            _dialogueInput.ToggleInputs(true);

            _isTopicJump = (_currentLine.slideToJump > 0) ? true : false;

            if(_secretEndingController != null || _secretEndingSlide > 0) { 
                if(_currentLine.slideIndex == _secretEndingSlide)
                    _secretEndingController.EnableButton();
                else
                    _secretEndingController.DisableButton();
            }

            _miniClassroomUI.SetConversation(_currentLine);
        }

        private void SetActors()
        {
            _miniClassroomUI.SetActors();
        }

        private void SetDialogue()
        {
            _miniClassroomUI.SetDialogue();
        }

        private void EndOFScene()
        {
            if (_alternativeEnding)
                EventManager.AlternativeEndingBoard();
            else
                EventManager.StoryBoardIsEnded();
        }

        private float GetAutoDelay()
        {
            int charCount = _currentLine.dialogueText.Length;

            return Mathf.Clamp(charCount * 0.05f, 1.5f, 5f);
        }

        private void StartAuto()
        {
            StopAuto();
            _autoMode = true;

            _autoCoroutine = StartCoroutine(AutoRoutine());
        }

        private void StopAuto()
        {
            _autoMode = false;

            if (_autoCoroutine != null)
            {
                StopCoroutine(_autoCoroutine);
                _autoCoroutine = null;
            }
        }

        private IEnumerator AutoRoutine()
        {
            yield return new WaitForSeconds(GetAutoDelay());

            if (!_autoMode) yield break;

            Next();
        }

        public void SwitchState(DialogueState state)
        {
            _currentState = state;

            switch (_currentState)
            {
                case DialogueState.StartDialog:
                    StartDialogue();
                    break;
                case DialogueState.ActorsAnimating:
                    SetActors();
                    break;
                case DialogueState.WritingDialog:
                    SetDialogue();
                    break;
                case DialogueState.DialogFinished:
                    if (_isTopicJump)
                    {
                        _dialogueInput.ToggleInputs(false);
                        _miniClassroomUI.ShowSkipTopicPanel();
                    }

                    if (_autoMode)
                        StartAuto();
                    break;
            }
        }

        public void StartConversation()
        {
            _miniClassroomUI.ClearConversation();

            _currentIndex = 0;
            _history.Clear();
            _history = new();
            SwitchState(DialogueState.StartDialog);
        }

        public void OnNextClick()
        {
            StopAuto();
            _miniClassroomUI.ToggleAutoRunButton();

            switch (_currentState)
            {
                case DialogueState.ActorsAnimating:
                    _miniClassroomUI.SkipActorsAnim();
                    break;
                case DialogueState.WritingDialog:
                    _miniClassroomUI.SkipDialogue();
                    break;
                case DialogueState.DialogFinished:
                    Next();
                    break;
            }
        }

        public void OnPreviousClick()
        {
            StopAuto();
            _miniClassroomUI.ToggleAutoRunButton();

            switch (_currentState)
            {
                case DialogueState.ActorsAnimating:
                    _miniClassroomUI.SkipActorsAnim(false);
                    Previous();
                    break;
                case DialogueState.WritingDialog:
                    _miniClassroomUI.SkipDialogue(false);
                    Previous();
                    break;
                case DialogueState.DialogFinished:
                    Previous();
                    break;
            }
        }

        public void ToggleAutoMode()
        {
            _autoMode = !_autoMode;

            if (_autoMode && _currentState == DialogueState.DialogFinished)
            {
                StartAuto();
            }

            if(!_autoMode)
                StopAuto();
        }

        public void Next()
        {
            if (_currentIndex >= _sequence.Count - 1)
                EndOFScene();
            else
            {
                _currentIndex++;
                SwitchState(DialogueState.StartDialog);
            }

        }

        public void Previous()
        {
            if (_currentIndex <= 0) return;

            var lineRemove = _sequence[_currentIndex];
            if(lineRemove != null)
                _history.Remove(lineRemove);

            _currentIndex--;
            var line = _sequence[_currentIndex];
            if (line != null)
                _history.Remove(line);
            SwitchState(DialogueState.StartDialog);
        }

        public void SkipTopic()
        {
            int index = _currentIndex;
            var slideToJump = _sequence[_currentIndex].slideToJump - 1;

            bool canSkip = false;
            for (int i = index + 1; i < _sequence.Count; i++)
            {
                if (_sequence[i].slideIndex == slideToJump)
                {
                    _currentIndex = i;
                    canSkip = true;
                    continue;
                }
            }

            if (canSkip)
            {
                _miniClassroomUI.HideSkipTopicPanel();
                Next();
            }
        }

        public void UnlockEnding()
        {
            _alternativeEnding = true;
        }

        public void FinishScene()
        {
            EndOFScene();
        }
    }
}
