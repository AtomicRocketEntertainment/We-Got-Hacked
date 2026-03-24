using UnityEngine;

namespace MiniClassRoom
{
    [System.Serializable]
    public class DialogueLine
    {
        public Actor actorLeft;
        public Actor actorLeftSecond;

        public Actor actorRight;
        public Actor actorRightSecond;

        public string actorHighlighted;

        public Sprite background;
    }
}
