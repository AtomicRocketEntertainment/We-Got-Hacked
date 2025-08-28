using UnityEngine;
using UnityEngine.UI;

public class AwaitingCardInstance : MonoBehaviour
{
    [SerializeField] private GameObject _micIcon;
    [SerializeField] private Image _profile;

    public void UpdateMyCard(Sprite profile)
    {
        _profile.sprite = profile;
        CloseMic();
    }

    public void OpenMic() => _micIcon.SetActive(false);
    public void CloseMic() => _micIcon.SetActive(true);
}
