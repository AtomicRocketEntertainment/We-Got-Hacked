using NaughtyAttributes;
using UnityEngine;

public class PDFScreen : MonoBehaviour, INeedOpenCanvas
{
    [BoxGroup("Canvases")] [SerializeField] private GameObject _mainCanvas;
    
    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }
}
