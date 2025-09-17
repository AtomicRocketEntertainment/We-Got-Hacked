using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DocumentButtonInstance : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private TextMeshProUGUI _name;
    private int _index = -1;
    private bool _shouldResize = false;
    public int Index => _index;

    private void OnEnable()
    {
        if (_shouldResize)
            StartCoroutine(DelayedResize());
    }

    public void Init(SO_DocumentButton infos, bool shouldResize)
    {
        _sprite.sprite = infos.Icon;
        _name.text = infos.Name;
        _index = infos.Index;
        _shouldResize = shouldResize;
    }

    private IEnumerator DelayedResize()
    {
        yield return null;
        ResizeButton();
    }

    private void ResizeButton()
    {
        float textHeight;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_name.rectTransform);
        _name.TryGetComponent(out RectTransform rect);

        textHeight = rect.sizeDelta.y; RectTransform thisRect = this.gameObject.GetComponent<RectTransform>();
        thisRect.sizeDelta = new Vector2(thisRect.sizeDelta.x, textHeight + 15f + 90f);
    }

}

