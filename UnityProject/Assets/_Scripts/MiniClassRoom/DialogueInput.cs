using UnityEngine;

namespace MiniClassRoom
{
    public class DialogueInput : MonoBehaviour
    {
        [SerializeField] private MiniClassRoomManager _manager;
        [SerializeField] private float _inputCooldown = 0.5f;

        private bool _inputsEnabled = true;
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
            if (!IsInputTimerEnable())
            {
                _inputTimer += Time.deltaTime;
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnPreviousClick();

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Space))
                OnNextClick();

        }

        private bool IsInputTimerEnable()
        {
            return _inputTimer >= _inputCooldown;
        }

        public void ToggleInputs(bool input)
        {
            _inputsEnabled = input;
        }

        public void OnNextClick()
        {
            if (!_inputsEnabled) return;
            if (!IsInputTimerEnable()) return;
            _manager.OnNextClick();
            _inputTimer = 0f;
        }

        public void OnPreviousClick()
        {
            if (!IsInputTimerEnable()) return;
            _manager.OnPreviousClick();
            _inputTimer = 0f;
        }
    }
}
