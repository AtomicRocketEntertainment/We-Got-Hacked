using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    [CreateAssetMenu(fileName = "Meeting Person", menuName = "Scriptable Objcts/MiniClass/Conversation")]
    public class ConversationSO : ScriptableObject
    {
        public List<DialogueLine> lines;
    }
}
