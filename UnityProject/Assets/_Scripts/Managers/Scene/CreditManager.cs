using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    [SerializeField] private GameObject _creditScreen;
    [SerializeField] private Image _currentImage;
    [SerializeField] private Sprite[] _creditsImage;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private float _timeToUpdate = 3f;
    [SerializeField] private float _fadeDuration = 1f;

    private int _index = 0;
    private Coroutine _routine;

    public void StartCredits()
    {
        _creditScreen.SetActive(true);
        _index = 0;

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(CreditsRoutine());
    }

    public void CloseCredits()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = null;
        _creditScreen.SetActive(false);
    }

    private IEnumerator CreditsRoutine()
    {
        if (_creditsImage.Length == 0)
        {
            CloseCredits();
            yield break;
        }

        _currentImage.sprite = _creditsImage[_index];
        yield return FadeIn();

        while (true)
        {
            yield return new WaitForSeconds(_timeToUpdate);

            yield return FadeOut();

            _index++;

            if (_index >= _creditsImage.Length)
            {
                CloseCredits();
                yield break;
            }

            _currentImage.sprite = _creditsImage[_index];

            yield return FadeIn();
        }
    }
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / _fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = 0f;
    }
}
