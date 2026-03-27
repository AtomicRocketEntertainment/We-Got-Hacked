using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    [System.Serializable]
    public class DialogueLine
    {
        public List<ActorData> actors; //Setting Actors in scene (only 3 - first is left, second is right, third is right second)

        public string actorHighlightedID;

        public Sprite background;
        public string dialogueText;
    }

    [System.Serializable]
    public class ActorData
    {
        public ActorSO actor;
        public string bodyID;
        public string headID;
    }
}
