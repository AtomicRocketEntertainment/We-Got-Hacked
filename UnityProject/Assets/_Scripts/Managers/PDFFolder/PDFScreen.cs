using NaughtyAttributes;
using UnityEngine;

public class PDFScreen : MonoBehaviour, INeedOpenCanvas
{
    [BoxGroup("Canvases"), SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Thinking Info"), SerializeField] private bool _shouldThinkFirstTimeOpen;

    private bool _alreadyThought;

    private void Awake()
    {
        _alreadyThought = !_shouldThinkFirstTimeOpen;
    }
    
    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);

        if (!_alreadyThought)
        {
            _alreadyThought = true;
            EventManager.MakePlayerThink(ThoughtKey.OpenPDFFirstTime);
        }
    }
}
