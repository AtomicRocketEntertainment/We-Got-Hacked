using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DocumentButtonInstance : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private TextMeshProUGUI _name;
    private int _index = -1;

    public int Index => _index;

    public void Init(SO_DocumentButton infos)
    {
        _sprite.sprite = infos.Icon;
        _name.text = infos.Name;
        _index = infos.Index;
    }
}
