using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemotopiaManager : MonoBehaviour, INeedOpenCanvas, ISoftwareContext
{
    [BoxGroup("Screens")][SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _accessScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _currentAccessedScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _txtScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _lockedScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _userDisconectedScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _quitPopup;

    [BoxGroup("Current txt displayed")][SerializeField] private TextMeshProUGUI _fileTxtText;
    [BoxGroup("Current txt displayed")][SerializeField] private TextMeshProUGUI _fileNameText;

    [BoxGroup("Current User infos Display")][SerializeField] private TextMeshProUGUI _userNameProfileDisplay;
    [BoxGroup("Current User infos Display")][SerializeField] private Image _userProfileDisplay;

    [BoxGroup("Login Dependencies")][SerializeField] private TMP_Dropdown _dropDownLogin;
    [BoxGroup("Login Dependencies")][SerializeField] private TextMeshProUGUI _userNameDisplay;
    [BoxGroup("Login Dependencies")][SerializeField] private Button _conectBtn;

    [BoxGroup("Quit popup")][SerializeField] private Button _confirmQuiting;
    [BoxGroup("Quit popup")][SerializeField] private Button _confirmDisconected;


    [BoxGroup("Instantiate Dependencies Documents")][SerializeField] private Transform _mainDocumentParent;
    [BoxGroup("Instantiate Dependencies Documents")][SerializeField] private Transform _sideDocumentParent;
    [BoxGroup("Instantiate Dependencies Documents")][SerializeField] private GameObject _btnMainDocumentprefab;
    [BoxGroup("Instantiate Dependencies Documents")][SerializeField] private GameObject _btnSideDocumentprefab;
    [BoxGroup("Instantiate Dependencies Documents")][SerializeField] private CurrentRemotopiaUser _currentUser;

    [BoxGroup("Other Dependencies")][SerializeField] private SO_TicketList[] _listOfTickets;
    [BoxGroup("Other Dependencies")][SerializeField] private SO_DocumentButtonList[] _listOfInitialBtns;

    private SoftwareState _currentState = SoftwareState.Blocked;
    private bool _selectedCorrectUser;
    private Dictionary<int, GameObject> _buttonsSpawned;
    private List<GameObject> _sideButtons; //needed to keep training to update later

    private readonly string _emailLore6Day2 = "Lore 6 Day 2";
    private readonly string _emailLore2Day2 = "Lore 2 Day 2";

    private void Awake()
    {
        _selectedCorrectUser = false;
        _conectBtn.interactable = false;
        _buttonsSpawned = new Dictionary<int, GameObject>();
        _sideButtons = new List<GameObject>();
        _dropDownLogin.ClearOptions();
    }

    void OnEnable()
    {
        EventManager.OnEventEmailHandlerIsOpen += CheckUpdates;
        _dropDownLogin.onValueChanged.AddListener(ChangeLoginUser);
        _confirmQuiting.onClick.AddListener(QuitUser);
        _confirmDisconected.onClick.AddListener(ConfirmDisconection);
        _conectBtn.onClick.AddListener(TryLogin);
    }

    void OnDisable()
    {
        EventManager.OnEventEmailHandlerIsOpen -= CheckUpdates;
        _dropDownLogin.onValueChanged.RemoveListener(ChangeLoginUser);
        _confirmQuiting.onClick.RemoveListener(QuitUser);
        _confirmDisconected.onClick.RemoveListener(ConfirmDisconection);
        _conectBtn.onClick.RemoveListener(TryLogin);
    }

    private void CheckUpdates(string emailIndex)
    {
        if (emailIndex == _emailLore2Day2)
            _currentState = SoftwareState.FullAccess;

        if (emailIndex == _emailLore6Day2)
        {
            _currentState = SoftwareState.FullAccess;
            _currentUser = CurrentRemotopiaUser.Remotopia_Server_Day_Two;
        }
    }

    private void CreateUsersToLogin()
    {
        if (_currentState == SoftwareState.Blocked)
            return;

        _dropDownLogin.ClearOptions();
        List<string> loginOptions = new List<string> { "" }; //first selected


        foreach (SO_Ticket ticket in _listOfTickets[(int)_currentUser].Tickets)
                loginOptions.Add(ticket.IPDestiny);

        _dropDownLogin.AddOptions(loginOptions);
    }

    private void ChangeLoginUser(int value)
    {
        string textToDisplay = "";
        bool userSelect = false;
        bool isValidUserToChange = value > 0 && value <= _listOfTickets[(int)_currentUser].Tickets.Count;

        if (isValidUserToChange)
        {
            RemotopiaUserLogin userSelected = _listOfTickets[(int)_currentUser].Tickets[value - 1].RemotopiaUser; 
            textToDisplay = userSelected.Name;
            _userProfileDisplay.sprite = userSelected.Sprite;
            _selectedCorrectUser = userSelected.IsCorrectUser;
            userSelect = true;
        }

        _userNameDisplay.text = textToDisplay;
        _userNameProfileDisplay.text = textToDisplay;
        _conectBtn.interactable = userSelect;
    }

    private void TryLogin()
    {
        if (!_selectedCorrectUser)
        {
            EventManager.WrongChoice();
            EventManager.MakePlayerThink(ThoughtKey.WrongIPOnRemotinik);
            return;
        }

        EventManager.LoginRemotopia();
        PrepareButtons();
        _accessScreen.SetActive(false);
        _currentAccessedScreen.SetActive(true);
    }

    private void PrepareButtons()
    {
        _buttonsSpawned.Clear();

        foreach (SO_DocumentButton button in _listOfInitialBtns[(int)_currentUser].Buttons)
        {
            GameObject newMainButton = Instantiate(_btnMainDocumentprefab, _mainDocumentParent, false);
            GameObject newSideButton = Instantiate(_btnSideDocumentprefab, _sideDocumentParent, false);
            newSideButton.name = button.Name;
            newMainButton.name = button.Name;

            newMainButton.TryGetComponent(out DocumentButtonInstance instance);
            newSideButton.TryGetComponent(out DocumentButtonInstance sideInstance);
            newMainButton.TryGetComponent(out Button btnMain);
            newSideButton.TryGetComponent(out Button btnSide);

            sideInstance.Init(button);
            instance.Init(button);

            _buttonsSpawned[button.Index] = newMainButton;
            _sideButtons.Add(newSideButton);

            if (button.IsFolder())
            {
                if (_currentUser == CurrentRemotopiaUser.Remotopia_Raquel_Day_Two)
                {
                    btnMain.onClick.AddListener(ManagerTestWasClicked);
                    btnSide.onClick.AddListener(ManagerTestWasClicked);
                }
                else
                {
                    btnMain.onClick.AddListener(() => FolderClicked(button.FolderButtons));
                    btnSide.onClick.AddListener(() => FolderClicked(button.FolderButtons));
                }


                foreach (SO_DocumentButton newButton in button.FolderButtons)
                {
                    GameObject archive = Instantiate(_btnMainDocumentprefab, _mainDocumentParent, false);
                    archive.name = button.Name;
                    archive.TryGetComponent(out DocumentButtonInstance archiveInstance);
                    archive.TryGetComponent(out Button btnArchive);
                    archiveInstance.Init(newButton);
                    archive.SetActive(false);

                    btnArchive.onClick.AddListener(() => ArchiveClicked(newButton.Type, newButton.Text, button.Name));

                    _buttonsSpawned[newButton.Index] = archive;
                }
            }
            else //Is archive, either txt or locked one
            {
                if (_currentUser == CurrentRemotopiaUser.Remotopia_Raquel_Day_Two)
                {
                    btnMain.onClick.AddListener(ManagerTestWasClicked);
                    btnSide.onClick.AddListener(ManagerTestWasClicked);
                }
                else
                {
                    btnMain.onClick.AddListener(() => ArchiveClicked(button.Type, button.Text, button.Name));
                    btnSide.onClick.AddListener(() => ArchiveClicked(button.Type, button.Text, button.Name));
                }
            }
        }
    }

    private void ManagerTestWasClicked()
    {
        _userDisconectedScreen.SetActive(true);
    }

    private void ConfirmDisconection()
    {
        EventManager.SpawnEmail(EmailType.LORE);
        QuitUser();
    }

    private void FolderClicked(List<SO_DocumentButton> listButtons)
    {
        foreach (var kvp in _buttonsSpawned)
            kvp.Value.SetActive(false);


        foreach (SO_DocumentButton button in listButtons)
        {
            if (_buttonsSpawned.TryGetValue(button.Index, out GameObject instance))
                instance.SetActive(true);
        }
    }

    private void ArchiveClicked(DocType type, string textToChange, string fileName)
    {
        switch (type)
        {
            case DocType.TXT:
                _fileTxtText.text = textToChange;
                _fileNameText.text = $"{fileName} - Bloco de Notas";
                _txtScreen.SetActive(true);
                _lockedScreen.SetActive(false);
                break;

            case DocType.BLOCKED:
                _lockedScreen.SetActive(true);
                _txtScreen.SetActive(false);
                break;
        }
    }


    private void QuitUser()
    {
        _currentState = SoftwareState.Blocked;
        _selectedCorrectUser = false;
        _conectBtn.interactable = false;
        _dropDownLogin.ClearOptions();

        foreach (var kvp in _buttonsSpawned)
            Destroy(kvp.Value);

        foreach (var sideButton in _sideButtons)
            Destroy(sideButton);

        _accessScreen.SetActive(true);
        _userDisconectedScreen.SetActive(false);
        _currentAccessedScreen.SetActive(false);
        _txtScreen.SetActive(false);
        _lockedScreen.SetActive(false);
        _quitPopup.SetActive(false);
        EventManager.QuitRemotopia();

        if (_currentUser == CurrentRemotopiaUser.Remotopia_Server_Day_Two)
            EventManager.SpawnEmail(EmailType.LORE);
    }

    public void ChangeSoftwareState(SoftwareState state)
    {
        _currentState = state;
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
        CreateUsersToLogin();
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }


    public void ChangeBlockedCanvasStatus(bool status)
    {
        //dont need it
    }
}

public enum CurrentRemotopiaUser
{
    Remotopia_Raquel_Day_Two, Remotopia_Server_Day_Two
}

[System.Serializable]
public struct RemotopiaUserLogin
{
    public string Name;
    public Sprite Sprite;
    public bool IsCorrectUser;
}
