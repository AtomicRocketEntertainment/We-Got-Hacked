using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    public class MiniClassroomUI : MonoBehaviour
    {
        [SerializeField] private List<ActorUI> _actorsUI;
        [SerializeField] private TextBoxUI _textBox;

        public void SetConversation(DialogueLine line)
        {
            ClearConversation();

            if (line.actors != null && line.actors.Count > 0)
            {
                int actorCount = 0;
                foreach(ActorData actor in line.actors)
                {
                    _actorsUI[actorCount].SetActor(actor.actor, actor.headID, actor.bodyID);
                    actorCount++;
                }
            }

            _textBox.SetDialogue(line.dialogueText, line.actorHighlightedID);
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
