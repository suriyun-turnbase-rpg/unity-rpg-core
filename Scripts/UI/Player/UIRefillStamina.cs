using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIRefillStamina : MonoBehaviour
{
    public enum StaminaType
    {
        Stage,
        Arena,
        Custom
    }

    public StaminaType type;
    public string customStaminaDataId;
    public UnityEvent eventRefillSuccess;
    public UnityEvent eventRefillFail;

    public void OnClickRefill()
    {
        string staminaDataId = customStaminaDataId;
        switch (type)
        {
            case StaminaType.Stage:
                staminaDataId = GameInstance.GameDatabase.stageStamina.id;
                break;
            case StaminaType.Arena:
                staminaDataId = GameInstance.GameDatabase.arenaStamina.id;
                break;
        }
        GameInstance.Singleton.ShowConfirmDialog(
            LanguageManager.GetText(GameText.WARN_TITLE_REFILL_STAMINA),
            LanguageManager.GetText(GameText.WARN_DESCRIPTION_REFILL_STAMINA),
            () =>
            {
                GameInstance.GameService.RefillStamina(staminaDataId, OnRefillSuccess, OnRefillFail);
            });
    }

    private void OnRefillSuccess(RefillStaminaResult result)
    {
        GameInstance.Singleton.OnGameServiceRefillStaminaResult(result);
        if (eventRefillSuccess != null)
            eventRefillSuccess.Invoke();
    }

    private void OnRefillFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventRefillFail != null)
            eventRefillFail.Invoke();
    }
}
