using System.Collections.Generic;
using UnityEngine;

public class BaseGamePlayFormation : MonoBehaviour
{
    [System.Serializable]
    public struct ContainerData
    {
        [Tooltip("Container which character will be stayed")]
        public Transform container;
        [Tooltip("Position priority, higher is more far, if character attack mode is attacking by priority it will attack near character first (low priority)")]
        public int priority;
    }

    public BaseGamePlayManager Manager { get { return BaseGamePlayManager.Singleton; } }
    // Deprecating
    [HideInInspector]
    public Transform[] containers;
    [HideInInspector]
    public Transform helperContainer;

    public List<ContainerData> characterContainers = new List<ContainerData>();
    public ContainerData helperCharacterContainer = default;

    public UIFormationCharacters uiFormationCharacters;
    public readonly Dictionary<int, BaseCharacterEntity> Characters = new Dictionary<int, BaseCharacterEntity>();

    private void Awake()
    {
        Migrate();
    }

    private void OnValidate()
    {
        Migrate();
    }

    private void Migrate()
    {
        if (containers != null && containers.Length > 0)
        {
            for (int i = 0; i < containers.Length; ++i)
            {
                if (i < characterContainers.Count)
                {
                    var tempContainer = characterContainers[i];
                    tempContainer.container = containers[i];
                    characterContainers[i] = tempContainer;
                }
                else
                {
                    var tempContainer = new ContainerData()
                    {
                        container = containers[i],
                    };
                    characterContainers.Add(tempContainer);
                }
            }
        }

        if (helperContainer != null)
        {
            var tempContainer = helperCharacterContainer;
            tempContainer.container = helperContainer;
            helperCharacterContainer = tempContainer;
        }
    }

    public virtual void SetFormationCharacters(EBattleType battleType)
    {
        var formationName = Player.CurrentPlayer.SelectedFormation;
        if (battleType == EBattleType.Arena)
            formationName = Player.CurrentPlayer.SelectedArenaFormation;
        ClearCharacters();
        for (var i = 0; i < containers.Length; ++i)
        {
            PlayerFormation playerFormation;
            if (PlayerFormation.TryGetData(formationName, i, out playerFormation))
            {
                var itemId = playerFormation.ItemId;
                PlayerItem item;
                if (!string.IsNullOrEmpty(itemId) && PlayerItem.DataMap.TryGetValue(itemId, out item))
                {
                    SetCharacter(i, item, false);
                }
            }
        }
        if (BaseGamePlayManager.Helper != null &&
            !string.IsNullOrEmpty(BaseGamePlayManager.Helper.MainCharacter) &&
            GameInstance.GameDatabase.Items.ContainsKey(BaseGamePlayManager.Helper.MainCharacter) &&
            helperContainer != null)
        {
            var item = new PlayerItem();
            item.Id = "_Helper";
            item.DataId = BaseGamePlayManager.Helper.MainCharacter;
            item.Exp = BaseGamePlayManager.Helper.MainCharacterExp;
            SetHelperCharacter(item);
        }
    }

    public void SetFormationCharactersForStage()
    {
        SetFormationCharacters(EBattleType.Stage);
    }

    public void SetFormationCharactersForArena()
    {
        SetFormationCharacters(EBattleType.Arena);
    }

    public void SetFormationCharactersForRaidBoss()
    {
        SetFormationCharacters(EBattleType.RaidBoss);
    }

    public void SetFormationCharactersForClanBoss()
    {
        SetFormationCharacters(EBattleType.ClanBoss);
    }

    public virtual void SetCharacters(PlayerItem[] items, List<int> bossIndexes = null)
    {
        ClearCharacters();
        for (var i = 0; i < containers.Length; ++i)
        {
            if (items.Length <= i)
                break;
            var item = items[i];
            SetCharacter(i, item, bossIndexes != null && bossIndexes.Contains(i));
        }
        if (uiFormationCharacters != null)
            uiFormationCharacters.SetFormation(this);
    }

    public virtual BaseCharacterEntity SetCharacter(int position, PlayerItem item, bool isBoss)
    {
        if (position < 0 || position >= containers.Length || item == null || item.CharacterData == null)
            return null;

        if (item.CharacterData.model == null)
        {
            Debug.LogWarning("Character's model is empty, this MUST be set");
            return null;
        }

        var container = containers[position];
        container.RemoveAllChildren();

        var character = Instantiate(item.CharacterData.model);
        character.SetFormation(this, position, container);
        character.Item = item;
        character.IsBoss = isBoss;
        Characters[position] = character;

        return character;
    }

    public virtual BaseCharacterEntity SetHelperCharacter(PlayerItem item)
    {
        if (helperContainer == null)
            return null;

        var position = containers.Length;

        if (item.CharacterData.model == null)
        {
            Debug.LogWarning("Character's model is empty, this MUST be set");
            return null;
        }

        var container = helperContainer;
        container.RemoveAllChildren();

        var character = Instantiate(item.CharacterData.model);
        character.SetFormation(this, position, container);
        character.Item = item;
        Characters[position] = character;

        return character;
    }

    public virtual void ClearCharacters()
    {
        foreach (var container in containers)
        {
            container.RemoveAllChildren();
        }
        if (helperContainer != null)
            helperContainer.RemoveAllChildren();
        Characters.Clear();
    }
}
