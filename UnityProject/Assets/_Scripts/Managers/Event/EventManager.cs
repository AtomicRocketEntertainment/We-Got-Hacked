using System;
using UnityEngine;

public static class EventManager
{
    //Email Related
    public static event Action<GameObject> OnOpenEmail;
    public static event Action<string> OnLinkIsClicked;
    public static event Action<string> OnChangeEmailContentText;
    public static event Action<Email> OnEmailResponseNeeded;
    public static event Action OnEmailIsAnswered;
    public static event Action OnReturnEmailContent;

    //Global Gaming Mechanic
    public static event Action OnCorrectChoice;
    public static event Action OnWrongChoice;

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

    public static void ChangeEmailTextContent(string newText)
    {
        if (OnChangeEmailContentText != null)
        {
            OnChangeEmailContentText(newText);
        }
        else
        {
            Debug.LogWarning("No listeners for OnChangeEmailContentText event.");
        }
    }

    public static void OpenEmailResponse(Email email)
    {
        if (OnEmailResponseNeeded != null)
        {
            OnEmailResponseNeeded(email);
        }
        else
        {
            Debug.LogWarning("No listeners for OnEmailResponseNeeded event.");
        }
    }

    public static void EmailIsAnswered()
    {
        if (OnEmailIsAnswered != null)
        {
            OnEmailIsAnswered();
        }
        else
        {
            Debug.LogWarning("No listeners for OnEmailIsAnswered event.");
        }
    }

    public static void ReturnEmailContent()
    {
        if (OnReturnEmailContent != null)
        {
            OnReturnEmailContent();
        }
        else
        {
            Debug.LogWarning("No listeners for OnReturnEmailContent event.");
        }
    }

    public static void CorrectChoice()
    {
        if (OnCorrectChoice != null)
        {
            OnCorrectChoice();
        }
        else
        {
            Debug.LogWarning("No listeners for OnCorrectChoice event.");
        }
    }

    public static void WrongChoice()
    {
        if (OnWrongChoice != null)
        {
            OnWrongChoice();
        }
        else
        {
            Debug.LogWarning("No listeners for OnWrongChoice event.");
        }
    }
}