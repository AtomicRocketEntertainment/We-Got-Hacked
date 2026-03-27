using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private Image _spriteBody;
        [SerializeField] private Image _spriteHead;

        private ActorSO _actorSO;

        public void SetActor(ActorSO actorSO, string bodyID, string headID)
        {
            _actorSO = actorSO;
            gameObject.SetActive(true);

            _spriteBody.sprite = _actorSO.GetBodySprite(bodyID);
            _spriteHead.sprite = _actorSO.GetHeadSprite(headID);
        }

        public void ClearActor()
        {
            _actorSO = null;
            _spriteBody.sprite = null;
            _spriteHead.sprite = null;
            gameObject.SetActive(false);

        }
    }
}