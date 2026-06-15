using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    [System.Serializable]
    public class DialogueLine
    {
        public List<ActorData> actors; //Setting Actors in scene (only 3 - first is left, second is right, third is right second)

        public string actorHighlightedID;

        public Sprite slide;
        public int slideIndex;
        public string dialogueText;

        public int slideToJump = 0; //pular tópico

        public ActorSO GetHighlightedActor()
        {
            if (actors == null || actors.Count == 0) return null;
            foreach (var actor in actors)
            {
                if (actor.actor.id == actorHighlightedID)
                {
                    return actor.actor;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class ActorData
    {
        public ActorSO actor;
        public string headID;
        public string bodyID;
    }
}
