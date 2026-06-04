using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace MiniClassRoom
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private Image _spriteHead;
        [SerializeField] private Image _spriteBody;

        [SerializeField] private Color _colorNormal = new Color(1,1,1);
        [SerializeField] private Color _colorFaded = new Color(0.827451f, 0.827451f, 0.827451f);

        [SerializeField] private float _animDuration = 1f;
        [SerializeField] private float _animPosition = 800f;

        [SerializeField] private bool _leftActor;

        private ActorSO _actorSO;

        private Tween _currentTween;

        public string ActorID => _actorSO != null ? _actorSO.id : "";

        public Action OnAnimationComplete;

        public void SetActor(ActorSO actorSO, string headID, string bodyID)
        {
            bool firstActor = (_actorSO == null);
            _actorSO = actorSO;
            gameObject.SetActive(true);

            _currentTween = null;

            Sprite body = _actorSO.GetBodySprite(bodyID);
            Sprite head = _actorSO.GetHeadSprite(headID);
            
            if (body != null) _spriteBody.sprite = body;
            if(head != null) _spriteHead.sprite = head;

            if (!firstActor) return;
            PlayEnterAnimation();
        }

        public void SetHighlight(bool isHighlighted)
        {
            float targetScale = isHighlighted ? 1.1f : 1f;
            Color targetColor = isHighlighted ? _colorNormal : _colorFaded;
            float scaleLeft = _leftActor ? 1f : -1f;

            _rectTransform.DOScale(new Vector3(targetScale * scaleLeft, targetScale), 0.2f).SetEase(Ease.OutQuad);
            _spriteHead.color = targetColor;
            _spriteBody.color = targetColor;
        }

        public void SetNormalActor()
        {
            float scaleLeft = _leftActor ? 1f : -1f;
            _rectTransform.DOScale(new Vector3(scaleLeft, 1f), 0.2f).SetEase(Ease.OutQuad);
            _spriteHead.color = _colorNormal;
            _spriteBody.color = _colorNormal;
        }

        public void PlayEnterAnimation()
        {
            float startX = _leftActor ? -_animPosition : _animPosition;

            // posição inicial fora da tela
            _rectTransform.anchoredPosition = new Vector2(startX, _rectTransform.anchoredPosition.y);

            // invisível
            _canvasGroup.alpha = 0;

            // anima
            Sequence seq = DOTween.Sequence();

            seq.Join(_rectTransform.DOAnchorPosX(0, _animDuration).SetEase(Ease.OutCubic));
            seq.Join(_canvasGroup.DOFade(1, _animDuration));

            seq.OnComplete(() =>
            {
                _currentTween = null;
                OnAnimationComplete?.Invoke();
            });

            _currentTween = seq;
        }

        public void SkipAnimation()
        {
            if(_currentTween == null) return;
            _currentTween?.Kill(true);
        }

        public void RemoveActor(Action onComplete = null)
        {
            if (_actorSO == null)
            {
                _currentTween = null;
                onComplete?.Invoke();
                return;
            }
            Sequence seq = DOTween.Sequence();

            float endX = _leftActor ? -_animPosition : _animPosition;
            seq.Join(_canvasGroup.DOFade(0, _animDuration));
            seq.Join(_rectTransform.DOAnchorPosX(endX, _animDuration).SetEase(Ease.OutCubic));

            seq.OnComplete(() =>
            {
                ClearActor();
                onComplete?.Invoke();
                OnAnimationComplete?.Invoke();
            });

            _currentTween = seq;
        }

        public void ClearActor()
        {
            _actorSO = null;
            _currentTween = null;
            _spriteBody.sprite = null;
            _spriteHead.sprite = null;
            gameObject.SetActive(false);
        }
    }
}