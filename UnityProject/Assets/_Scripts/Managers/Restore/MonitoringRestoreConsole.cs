using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class MonitoringRestoreConsole : MonoBehaviour
{
    [SerializeField] private GameObject _holdingScreen;
    [SerializeField] private GameObject _infosScreen;
    [SerializeField] private GameObject _copyDisplayScreen;

    [BoxGroup("Console Dependencies"), SerializeField] private Button _closeCopy;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _timestampFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _originFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _destinyFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _protocolFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _archiveFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _contentFields;

    [BoxGroup("Console Dependencies"), SerializeField] private Button[] _copyBtns;

    [BoxGroup("Copy Configuration"), SerializeField] private float _timeToAnimate = 0.5f;
    [BoxGroup("Copy Configuration"), SerializeField] private LeanTweenType _tween = LeanTweenType.linear;

    private readonly float _copyXTranslate = 48f;
    private readonly float _returnCopyTranslate = -440f;
    void OnEnable()
    {
        _holdingScreen.SetActive(true);
        _infosScreen.SetActive(false);

        _closeCopy.onClick.AddListener(CloseCopyDisplay);
        EventManager.OnConsoleOpened += OpenConsole;
    }

    void OnDisable()
    {
        EventManager.OnConsoleOpened -= OpenConsole;
        _closeCopy.onClick.RemoveListener(CloseCopyDisplay);
        ForceCloseCopy();
    }

    private void OpenConsole(SO_ConsoleInfos infos)
    {
        _holdingScreen.SetActive(false);
        _infosScreen.SetActive(true);
        for (int i = 0; i < infos.Size; i++)
        {
            int index = i;
            _timestampFields[i].SetText(infos.Timestamp[i]);
            _originFields[i].SetText(infos.Origin[i]);
            _destinyFields[i].SetText(infos.Destiny[i]);
            _protocolFields[i].SetText(infos.Protocol[i]);
            _archiveFields[i].SetText(infos.Archive[i]);
            _contentFields[i].SetText(infos.Content[i]);

            _copyBtns[index].onClick.AddListener(() => CopyClicked(infos.Content[index]));
        }
    }

    private void CopyClicked(string key)
    {
        //readed by binary 64
        ShowCopyDisplay();
        EventManager.CopyConsole(key);
    }

    private void ShowCopyDisplay() => LeanTween.moveLocalX(_copyDisplayScreen, _copyXTranslate, _timeToAnimate).setEase(_tween);
    private void CloseCopyDisplay() => LeanTween.moveLocalX(_copyDisplayScreen, _returnCopyTranslate, _timeToAnimate).setEase(_tween);
    private void ForceCloseCopy() => _copyDisplayScreen.transform.localPosition = new Vector3(_returnCopyTranslate, _copyDisplayScreen.transform.localPosition.y, _copyDisplayScreen.transform.localPosition.z);
}
