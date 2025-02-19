using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailInstance : MonoBehaviour
{
    [SerializeField] private Button _emailButton;
    [SerializeField] private GameObject _newBg;
    [SerializeField] private Image _emailBackground;
    [SerializeField] private Color _openedEmailColor;
    [SerializeField] private TextMeshProUGUI _sender;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _contentSmall;

    private void OnEnable()
    {
        _emailButton.onClick.AddListener(ClickEmail);
    }

    private void OnDisable()
    {
        _emailButton.onClick.RemoveListener(ClickEmail);
    }

    public void UpdateInfos(string sender, string title, string contentSmall)
    {
        _sender.text = sender;
        _title.text = title;
        _contentSmall.text = contentSmall;
    }

    private void ClickEmail()
    {
        _newBg.SetActive(false);
        _emailBackground.color = _openedEmailColor;
        EventManager.OpenEmail(this.gameObject);
    }
}
