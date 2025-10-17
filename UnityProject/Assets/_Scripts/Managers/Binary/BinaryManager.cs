using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BinaryManager : MonoBehaviour, INeedOpenCanvas, IChoiceContext
{
    [SerializeField] private GameObject _mainCanvas;

    [BoxGroup("Buttons"), SerializeField] private Button _binary32Btn;
    [BoxGroup("Buttons"), SerializeField] private Button _binary64Btn;
    [BoxGroup("Buttons"), SerializeField] private Button _hexBtn;

    [BoxGroup("Content Main"), SerializeField] private TextMeshProUGUI _pageTitle;
    [BoxGroup("Content Main"), SerializeField] private Button _pasteBtn;
    [BoxGroup("Content Main"), SerializeField] private Button _decryptBtn;
    [BoxGroup("Content Main"), SerializeField] private Button _cleanFieldBtn;

    [BoxGroup("Upper Content"), SerializeField] private TextMeshProUGUI _pastedMessage;
    [BoxGroup("Upper Content"), SerializeField] private GameObject _pasteScreen;
    [BoxGroup("Upper Content"), SerializeField] private GameObject _sampleTextScreen;

    [BoxGroup("Lower Content"), SerializeField] private TextMeshProUGUI _decryptedMessage;

    [BoxGroup("Configs"), SerializeField] private string _message32 = "<color=#40C79C>Base32</color> para texto";
    [BoxGroup("Configs"), SerializeField] private string _message64 = "<color=#40C79C>Base64</color> para texto";
    [BoxGroup("Configs"), SerializeField] private string _messageHex = "<color=#40C79C>Hexadecimal</color> para texto";
    [BoxGroup("Configs"), SerializeField] private string _wrongMessage32 = "<color=red>Error:</color> Invalid base32 characters";
    [BoxGroup("Configs"), SerializeField] private string _wrongMessage64 = "<color=red>Error:</color> Invalid base64 characters";
    [BoxGroup("Configs"), SerializeField] private string _wrongMessageHex = "<color=red>Error:</color> Invalid hex characters";

    [SerializeField, BoxGroup("State Fluxogram")] private HistoryPartState _currentChoiceState = HistoryPartState.Part_One;
    [SerializeField, BoxGroup("State Fluxogram")] private Character _currentCharacter = Character.None;

    private Dictionary<(Character, HistoryPartState), IChoiceStateHandler> _choiceStateHandlers;

    private bool _isCorrectSearch;
    private bool _isCorrectMessage;
    private bool _canDecrypt;
    private string _currentCryptedMessage;
    private string _wrongMessage;

    private void Awake()
    {
        _isCorrectSearch = false;
        _canDecrypt = false;
        _currentCryptedMessage = "";
        _wrongMessage = _wrongMessage32;

        
        IChoiceStateSetup choiceSetup = _currentCharacter switch
        {
            Character.Eduardo_Day_Three => new Day_Three_ChoiceStateSetupBinary_Eduardo(),
            _ => null
        };

        choiceSetup?.RegisterStates(_choiceStateHandlers);
    }

    private void OnEnable()
    {
        EventManager.OnConsoleInfoCopied += UpdatePasteInfo;

        _cleanFieldBtn.onClick.AddListener(ResetSearch);
        _decryptBtn.onClick.AddListener(DecryptedMessage);
        _pasteBtn.onClick.AddListener(TryPaste);
        _binary32Btn.onClick.AddListener(() => ChangeScreen(_message32, _wrongMessage32, isCorrectSearch: false));
        _binary64Btn.onClick.AddListener(() => ChangeScreen(_message64, _wrongMessage64, isCorrectSearch: true));
        _hexBtn.onClick.AddListener(() => ChangeScreen(_messageHex, _wrongMessageHex, isCorrectSearch: false));
    }

    private void TryPaste()
    {
        if (_currentCryptedMessage == string.Empty) return;

        _canDecrypt = true;
        _pasteScreen.SetActive(false);
        _sampleTextScreen.SetActive(true);
    }

    private void OnDisable()
    {
        EventManager.OnConsoleInfoCopied -= UpdatePasteInfo;

        _cleanFieldBtn.onClick.RemoveAllListeners();
        _decryptBtn.onClick.RemoveAllListeners();
        _pasteBtn.onClick.RemoveAllListeners();
        _binary32Btn.onClick.RemoveAllListeners();
        _binary64Btn.onClick.RemoveAllListeners();
        _hexBtn.onClick.RemoveAllListeners();
    }

    private void ChangeScreen(string text, string wrongMessage, bool isCorrectSearch)
    {
        _isCorrectSearch = isCorrectSearch;
        _wrongMessage = wrongMessage;
        ResetSearch();
        _pageTitle.SetText(text);
    }

    private void ResetSearch()
    {
        _decryptedMessage.SetText(string.Empty);
        _canDecrypt = false;
        _pasteScreen.SetActive(true);
        _sampleTextScreen.SetActive(false);
    }

    private void DecryptedMessage()
    {
        if (!_canDecrypt) return;

        bool playerIsCorrect = _isCorrectMessage && _isCorrectSearch;

        if (playerIsCorrect)
            HandleState();
        else
            EventManager.WrongChoice();

        _decryptedMessage.SetText(playerIsCorrect ? _currentCryptedMessage : _wrongMessage);
    }

    private void UpdatePasteInfo(ConsoleContent content)
    {
        _isCorrectMessage = content.IsCorrect;
        _currentCryptedMessage = content.DecryptedMessage;
    }

    private void HandleState()
    {
        if (_choiceStateHandlers.TryGetValue((_currentCharacter, _currentChoiceState), out var handler))
        {
            handler.Handle(this);
        }
        else
        {
            Debug.LogWarning("Nenhum handler para este estado/personagem.");
        }
    }

    public void CloseCanvas() => _mainCanvas.SetActive(false);
    public void OpenCanvas() => _mainCanvas.SetActive(true);

    public void ChangeChoiceState(HistoryPartState state)
    {
        _currentChoiceState = state;
    }
}
