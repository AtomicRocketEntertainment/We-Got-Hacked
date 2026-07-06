using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class SecretEndingController : MonoBehaviour
    {
        [SerializeField] private MiniClassRoomManager _miniClassRoomManager;
        [SerializeField] private Image _secretButtonImage;
        [SerializeField] private Button _secretButton;
        [SerializeField] private int _clicksNeeded = 3;

        private int _currentClicks;

        public bool HasUnlockedEnding = false;

        private void Start()
        {
            _secretButton.onClick.AddListener(SecretClick);
            _secretButton.interactable = false;
            HasUnlockedEnding = false;
        }

        private void SecretClick()
        {
            if (HasUnlockedEnding)
                return;

            _currentClicks++;

            Debug.Log($"Clique {_currentClicks}/{_clicksNeeded}");

            if (_currentClicks >= _clicksNeeded)
            {
                Debug.Log("Final alternativo desbloqueado!");
                HasUnlockedEnding = true;
                DisableButton();
                _miniClassRoomManager.UnlockEnding();
            }
        }
        public void EnableButton()
        {
            if(HasUnlockedEnding)
                return;

            _secretButtonImage.raycastTarget = true;
            _secretButton.interactable = true;
        }

        public void DisableButton()
        {
            _currentClicks = 0;
            _secretButtonImage.raycastTarget = false;
            _secretButton.interactable = false;
        }
    }
}
