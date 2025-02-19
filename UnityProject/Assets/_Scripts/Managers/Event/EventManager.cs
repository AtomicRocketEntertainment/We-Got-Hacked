using System;
using UnityEngine;

public static class EventManager
{
    public static event Action<GameObject> OnOpenEmail;
    public static event Action<string> OnLinkIsClicked;

    public static void OpenEmail(GameObject emailObject)
    {
        if (OnOpenEmail != null)
        {
            OnOpenEmail(emailObject);
        }
        else
        {
            Debug.LogWarning("No listeners for OnOpenEmail event.");
        }
    }

    public static void ClickLink(string linkID)
    {
        if (OnLinkIsClicked != null)
        {
            OnLinkIsClicked(linkID);
        }
        else
        {
            Debug.LogWarning("No listeners for OnLinkIsClicked event.");
        }
    }
}