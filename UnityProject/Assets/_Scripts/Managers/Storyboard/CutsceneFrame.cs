using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneFrame : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textGui;
    [HideInInspector] public bool IsLastFrame = false; 
    private string _text;
    
    public void SetFrame(Sprite sprite, string text, bool isLastFrame)
    {
        IsLastFrame = isLastFrame;
        _image.sprite = sprite;
        _image.color = new Color(1, 1, 1, 0);
        _text = text;
    }

    public void ShowFrame()
    {
        if(_text != "")
            _textGui.text = _text;

        LeanTween.alpha(_image.rectTransform, 1f, 0.5f);
    }
    
}
