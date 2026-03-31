using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class MiniClassroomUI : MonoBehaviour
    {
        [SerializeField] private List<ActorUI> _actorsUI;
        [SerializeField] private Image _slideImg;
        [SerializeField] private TextBoxUI _textBox;

        DialogueLine _currentLine;
        ActorSO _currentActorHighlighted;

        private void SetActors()
        {
            if (_currentLine.actors != null && _currentLine.actors.Count > 0)
            {
                int actorCount = 0;
                foreach (ActorUI actor in _actorsUI)
                {
                    if (actorCount >= _currentLine.actors.Count)
                    {
                        actor.RemoveActor();
                        continue;
                    }

                    actor.SetActor(_currentLine.actors[actorCount].actor, _currentLine.actors[actorCount].headID, _currentLine.actors[actorCount].bodyID);
                    if(_currentLine.actorHighlightedID != null && _currentLine.actorHighlightedID != "")
                    {
                        bool isHighlighted = _currentLine.actors[actorCount].actor.id == _currentLine.actorHighlightedID;
                        actor.SetHighlight(isHighlighted);
                    }
                    else
                        actor.SetNormalActor();
                    actorCount++;
                }
            }
        }

        private void SetBackground()
        {
            if (_currentLine.background == null) return;
            if (_slideImg.sprite == _currentLine.background) return;
            _slideImg.sprite = _currentLine.background;
        }

        private void SetDialogue()
        {
            if(_currentActorHighlighted != null && _currentActorHighlighted.name != "")
                _textBox.SetActorName(_currentActorHighlighted.name);
            else
                _textBox.SetActorName();
            _textBox.SetDialogue(_currentLine.dialogueText);
        }

        private ActorSO GetHighlightedActor()
        {
            if (_currentLine.actors == null || _currentLine.actors.Count == 0) return null;
            foreach (var actor in _currentLine.actors)
            {
                if (actor.actor.id == _currentLine.actorHighlightedID)
                {
                    return actor.actor;
                }
            }
            return null;
        }

        public void SetConversation(DialogueLine line)
        {
            _currentLine = line;

            _currentActorHighlighted = GetHighlightedActor();

            SetActors();

            SetBackground();

            SetDialogue();
        }

        public void ClearConversation()
        {
            foreach(ActorUI actorUI in _actorsUI)
            {
                actorUI.ClearActor();
            }
            _textBox.ClearDialogue();
        }
    }
}
