using UnityEngine;

namespace MiniClassRoom
{
    public class DialogueInput : MonoBehaviour
    {
        [SerializeField] private MiniClassRoomManager _manager;
        [SerializeField] private float _inputCooldown = 0.5f;

        private float _inputTimer = 0f;

        private void Start()
        {
            _inputTimer = _inputCooldown;
        }

        void Update()
        {
            HandleInputs();
        }

        private void HandleInputs()
        {
            if (!IsInputEnable())
            {
                _inputTimer += Time.deltaTime;
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Space))
                OnNextClick();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnPreviousClick();
        }

        private bool IsInputEnable()
        {
            return _inputTimer >= _inputCooldown;
        }

        public void OnNextClick()
        {
            if (!IsInputEnable()) return;
            _manager.OnNextClick();
            _inputTimer = 0f;
        }

        public void OnPreviousClick()
        {
            if (!IsInputEnable()) return;
            _manager.OnPreviousClick();
            _inputTimer = 0f;
        }
    }
}
