using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    //Email Related
    public static event Action OnPlayerCantWriteEmail;
    public static event Action<GameObject> OnOpenEmail;
    public static event Action OnTryWriteEmail;
    public static event Action<Email> OnWriteEmail;
    public static event Action<string> OnEmailIsWriten;
    public static event Action<string> OnLinkIsClicked;
    public static event Action<string> OnChangeEmailContentText;
    public static event Action<Email, bool> OnEmailResponseNeeded;
    public static event Action<EmailType> OnSpawnEmail;
    public static event Action<SO_Email, bool, bool> OnCreateEspecificEmail;
    public static event Action<string> OnEmailIsAnswered;
    public static event Action OnReturnEmailContent;

    //Alert related
    public static event Action<Ticket, Color> OnAlertIsOpen;

    //Restore related
    public static event Action<List<TicketLog>> OnOpenLog;
    public static event Action<SiteBackup> OnOpenBackup;
    public static event Action<RestoreState> OnSiteIsOff;


    //Global Gaming Mechanic
    public static event Action OnEndStoryBoard;
    public static event Action OnStoryBoardNeeded;
    public static event Action OnCorrectChoice;
    public static event Action OnWrongChoice;
    public static event Action OnCloseResponseScreen;
    public static event Action<ThoughtKey> OnPlayerNeedToThink;
    public static event Action<GameObject> OnNotifyNeeded;
    public static event Action OnNotifyBrowser;

    //Lore related
    public static event Action OnFirstTimeSoftwareOpen;
    public static event Action OnCompletedTicketObjective;
    public static event Action<string> OnEventEmailHandlerIsOpen;
    public static event Action<int> OnTimerIsComplete;

    //Website Related
    public static event Action<string> OnWebsiteLinkerIsOpen;

    //Remotopia Related
    public static event Action OnUserEnterRemotopia;
    public static event Action OnUserQuitRemotopia;

    //Question Related
    public static event Action OnGenericResponseNeeded;
    public static event Action<string> OnGenericResponseIsMaded;

    //Persistence Data and Firebase Related
    public static event Action<PlayerDataAnswer> OnAnswerToSaveIsMaded;
    public static event Action<string> OnPlayerCreated;
    public static event Action<string> OnPlayerLoggedIn;
    public static event Action<string> OnAuthError;
    public static event Action<string> OnDatabaseEror;


    public static void BlockPlayerWriteEmail()
    {
        if (OnPlayerCantWriteEmail != null)
        {
            OnPlayerCantWriteEmail();
        }
        else
        {
            Debug.LogWarning("No listeners for OnPlayerCantWriteEmail event.");
        }
    }

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

    public static void WriteEmail(Email email)
    {
        if (OnWriteEmail != null)
        {
            OnWriteEmail(email);
        }
        else
        {
            Debug.LogWarning("No listeners for OnWriteEmail event.");
        }
    }

    public static void EmailIsWriten(string emailIndex)
    {
        if (OnEmailIsWriten != null)
        {
            OnEmailIsWriten(emailIndex);
        }
        else
        {
            Debug.LogWarning("No listeners for OnEmailIsWriten event.");
        }
    }

    public static void TryWriteEmail()
    {
        if (OnTryWriteEmail != null)
        {
            OnTryWriteEmail();
        }
        else
        {
            Debug.LogWarning("No listeners for OnTryWriteEmail event.");
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

    public static void OpenEmailResponse(Email email, bool isResponse)
    {
        if (OnEmailResponseNeeded != null)
        {
            OnEmailResponseNeeded(email, isResponse);
        }
        else
        {
            Debug.LogWarning("No listeners for OnEmailResponseNeeded event.");
        }
    }

    public static void SpawnEmail(EmailType type)
    {
        if (OnSpawnEmail != null)
        {
            OnSpawnEmail(type);
        }
        else
        {
            Debug.LogWarning("No listeners for OnSpawnEmail event.");
        }
    }

    public static void CreateEspecificEmail(SO_Email email, bool shouldAdvaneHistory, bool spawnOnTime)
    {
        if (OnCreateEspecificEmail != null)
        {
            OnCreateEspecificEmail(email, shouldAdvaneHistory, spawnOnTime);
        }
        else
        {
            Debug.LogWarning("No listeners for OnCreateEspecificEmail event.");
        }
    }

    public static void EmailIsAnswered(string emailIndex)
    {
        if (OnEmailIsAnswered != null)
        {
            OnEmailIsAnswered(emailIndex);
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

    public static void StoryBoardIsEnded()
    {
        if (OnEndStoryBoard != null)
        {
            OnEndStoryBoard();
        }
        else
        {
            Debug.LogWarning("No listeners for OnEndStoryBoard event.");
        }
    }

    public static void ShowStoryBoard()
    {
        if (OnStoryBoardNeeded != null)
        {
            OnStoryBoardNeeded();
        }
        else
        {
            Debug.LogWarning("No listeners for OnStoryBoardNeeded event.");
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

    public static void CloseResponseScreen()
    {
        if (OnCloseResponseScreen != null)
        {
            OnCloseResponseScreen();
        }
        else
        {
            Debug.LogWarning("No listeners for OnCloseResponseScreen event.");
        }
    }

    public static void OpenAlert(Ticket alert, Color color)
    {
        if (OnAlertIsOpen != null)
        {
            OnAlertIsOpen(alert, color);
        }
        else
        {
            Debug.LogWarning("No listeners for OnAlertIsOpen event.");
        }
    }

    public static void OpenLog(List<TicketLog> logList)
    {
        if (OnOpenLog != null)
        {
            OnOpenLog(logList);
        }
        else
        {
            Debug.LogWarning("No listeners for OnOpenLog event.");
        }
    }

    public static void OpenBackup(SiteBackup backup)
    {
        if (OnOpenBackup != null)
        {
            OnOpenBackup(backup);
        }
        else
        {
            Debug.LogWarning("No listeners for OnOpenBackup event.");
        }
    }

    public static void SiteIsOff(RestoreState state)
    {
        if (OnSiteIsOff != null)
        {
            OnSiteIsOff(state);
        }
        else
        {
            Debug.LogWarning("No listeners for OnSiteIsOff event.");
        }
    }

    public static void FirstTimeOpenSoftware()
    {
        if (OnFirstTimeSoftwareOpen != null)
        {
            OnFirstTimeSoftwareOpen();
        }
        else
        {
            Debug.LogWarning("No listeners for OnFirstTimeSoftwareOpen event.");
        }
    }

    public static void TicketObjectiveCompleted()
    {
        if (OnCompletedTicketObjective != null)
        {
            OnCompletedTicketObjective();
        }
        else
        {
            Debug.LogWarning("No listeners for OnCompletedTicketObjective event.");
        }
    }

    public static void EventEmailIsOpen(string emailIndex)
    {
        if (OnEventEmailHandlerIsOpen != null)
        {
            OnEventEmailHandlerIsOpen(emailIndex);
        }
        else
        {
            Debug.LogWarning("No listeners for OnEventEmailHandlerIsOpen event.");
        }
    }

    public static void TimerCompleted(int currentEvent)
    {
        if (OnTimerIsComplete != null)
        {
            OnTimerIsComplete(currentEvent);
        }
        else
        {
            Debug.LogWarning("No listeners for OnTimerIsComplete event.");
        }
    }

    public static void MakePlayerThink(ThoughtKey quote)
    {
        if (OnPlayerNeedToThink != null)
        {
            OnPlayerNeedToThink(quote);
        }
        else
        {
            Debug.LogWarning("No listeners for OnPlayerNeedToThink event.");
        }
    }

    public static void NotifyBar(GameObject obj)
    {
        if (OnNotifyNeeded != null)
        {
            OnNotifyNeeded(obj);
        }
        else
        {
            Debug.LogWarning("No listeners for OnNotifyNeeded event.");
        }
    }

    public static void NotifyBrowser()
    {
        if (OnNotifyBrowser != null)
        {
            OnNotifyBrowser();
        }
        else
        {
            Debug.LogWarning("No listeners for OnNotifyBrowser event.");
        }
    }

    public static void WebsiteIsOpen(string siteLink)
    {
        if (OnWebsiteLinkerIsOpen != null)
        {
            OnWebsiteLinkerIsOpen(siteLink);
        }
        else
        {
            Debug.LogWarning("No listeners for OnWebsiteLinkerIsOpen event.");
        }
    }

    public static void LoginRemotopia()
    {
        if (OnUserEnterRemotopia != null)
        {
            OnUserEnterRemotopia();
        }
        else
        {
            Debug.LogWarning("No listeners for OnUserEnterRemotopia event.");
        }
    }

    public static void QuitRemotopia()
    {
        if (OnUserQuitRemotopia != null)
        {
            OnUserQuitRemotopia();
        }
        else
        {
            Debug.LogWarning("No listeners for OnUserQuitRemotopia event.");
        }
    }

    public static void OpenGenericResponse()
    {
        if (OnGenericResponseNeeded != null)
        {
            OnGenericResponseNeeded();
        }
        else
        {
            Debug.LogWarning("No listeners for OnGenericResponseNeeded event.");
        }
    }

    public static void GenericResponseIsMade(string index)
    {
        if (OnGenericResponseIsMaded != null)
        {
            OnGenericResponseIsMaded(index);
        }
        else
        {
            Debug.LogWarning("No listeners for OnGenericResponseIsMaded event.");
        }
    }

    public static void AnswerToSaveIsMaded(PlayerDataAnswer answer)
    {
        if (OnAnswerToSaveIsMaded != null)
        {
            OnAnswerToSaveIsMaded(answer);
        }
        else
        {
            Debug.LogWarning("No listeners for OnAnswerToSaveIsMaded event.");
        }
    }

    public static void PlayerAreCreated(string feedbackMessage)
    {
        if (OnPlayerCreated != null)
        {
            OnPlayerCreated(feedbackMessage);
        }
        else
        {
            Debug.LogWarning("No listeners for OnPlayerCreated event.");
        }
    }

    public static void PlayerLoggedIn(string feedbackMessage)
    {
        if (OnPlayerLoggedIn != null)
        {
            OnPlayerLoggedIn(feedbackMessage);
        }
        else
        {
            Debug.LogWarning("No listeners for OnPlayerLoggedIn event.");
        }
    }

    public static void AuthError(string feedbackMessage)
    {
        if (OnAuthError != null)
        {
            OnAuthError(feedbackMessage);
        }
        else
        {
            Debug.LogWarning("No listeners for OnAuthError event.");
        }
    }
    
    public static void DatabaseError(string feedbackMessage)
    {
        if (OnDatabaseEror != null)
        {
            OnDatabaseEror(feedbackMessage);
        }
        else
        {
            Debug.LogWarning("No listeners for OnDatabaseEror event.");
        }
    }
}