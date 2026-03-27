using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private Image _spriteHead;
        [SerializeField] private Image _spriteBody;

        private ActorSO _actorSO;

        public void SetActor(ActorSO actorSO, string headID, string bodyID)
        {
            _actorSO = actorSO;
            gameObject.SetActive(true);

            Sprite body = _actorSO.GetBodySprite(bodyID);
            Sprite head = _actorSO.GetHeadSprite(headID);

            if(body != null) _spriteBody.sprite = body;
            if(head != null) _spriteHead.sprite = head;
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