using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class LogActorElement : LogElement
    {
        [SerializeField] private Image _actorCircle;
        [SerializeField] private TextMeshProUGUI _actorName;

        public override void Init(DialogueLine line)
        {
            base.Init(line);
            ActorSO actorSpeaker = line.GetHighlightedActor();

            _actorName.text = $"{actorSpeaker.actorName}";
            _actorName.color = actorSpeaker.colorAtorTextLog;
            _actorCircle.color = actorSpeaker.colorCircleAtorLog;
        }
    }
}
