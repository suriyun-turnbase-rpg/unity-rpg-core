using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUIArenaPreparation : UIBase
{
    public UIFormation uiCurrentFormation;
    public UIItem uiFormationSlotPrefab;
    public UIStamina uiRequireStamina;
    public UIArenaOpponentList uiArenaOpponentList;

    public override void Show()
    {
        base.Show();

        if (uiCurrentFormation != null)
        {
            uiCurrentFormation.formationName = Player.CurrentPlayer.SelectedArenaFormation;
            uiCurrentFormation.SetFormationData(uiFormationSlotPrefab);
        }

        if (uiRequireStamina != null)
        {
            var staminaData = PlayerStamina.ArenaStamina.Clone().SetAmount(1, 0);
            uiRequireStamina.SetData(staminaData);
        }
    }

    public void OnClickStartDuel()
    {
        BaseGamePlayManager.StartDuel(GetOpponent().Id);
    }

    public Player GetOpponent()
    {
        if (uiArenaOpponentList != null)
        {
            var list = uiArenaOpponentList.GetSelectedDataList();
            if (list.Count > 0)
                return list[0];
        }
        return null;
    }
}
