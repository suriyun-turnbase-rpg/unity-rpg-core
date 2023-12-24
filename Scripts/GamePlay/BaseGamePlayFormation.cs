using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseGamePlayFormation : MonoBehaviour
{
    [System.Serializable]
    public struct ContainerData
    {
        [Tooltip("Container which character will be stayed")]
        public Transform container;
        [Tooltip("Position priority, higher is more far, if character attack mode is attacking by priority it will attack near character first (low priority)")]
        public int priority;
        public CalculatedAttributes buffs;
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        Migrate();
        EditorUtility.SetDirty(this);
    }
#endif

    private bool Migrate()
    {
        bool hasChanges = false;
        if (containers != null && containers.Length > 0)
        {
            for (int i = 0; i < containers.Length; ++i)
            {
                if (i < characterContainers.Count)
                {
                    var tempContainer = characterContainers[i];
                    tempContainer.container = containers[i];
                    characterContainers[i] = tempContainer;
                    hasChanges = true;
                }
                else
                {
                    var tempContainer = new ContainerData()
                    {
                        container = containers[i],
                    };
                    characterContainers.Add(tempContainer);
                    hasChanges = true;
                }
            }
            containers = null;
        }

        if (helperContainer != null)
        {
            var tempContainer = helperCharacterContainer;
            tempContainer.container = helperContainer;
            helperCharacterContainer = tempContainer;
            helperContainer = null;
            hasChanges = true;
        }
        return hasChanges;
    }

    public virtual void SetFormationCharacters(EBattleType battleType)
    {
        var formationName = Player.CurrentPlayer.SelectedFormation;
        if (battleType == EBattleType.Arena)
            formationName = Player.CurrentPlayer.SelectedArenaFormation;
        ClearCharacters();
        for (var i = 0; i < characterContainers.Count; ++i)
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
            helperCharacterContainer.container != null)
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
        for (var i = 0; i < characterContainers.Count; ++i)
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
        if (position < 0 || position >= characterContainers.Count || item == null || item.CharacterData == null)
            return null;

        if (item.CharacterData.model == null)
        {
            Debug.LogWarning("Character's model is empty, this MUST be set");
            return null;
        }

        var container = characterContainers[position].container;
        container.RemoveAllChildren();

        var character = Instantiate(item.CharacterData.model);
        character.SetFormation(this, position, characterContainers[position].priority, container);
        character.Item = item;
        character.IsBoss = isBoss;
        CharacterBuffComponent buffComp = character.gameObject.AddComponent<CharacterBuffComponent>();
        buffComp.buffs = characterContainers[position].buffs;
        Characters[position] = character;

        return character;
    }

    public virtual BaseCharacterEntity SetHelperCharacter(PlayerItem item)
    {
        if (helperCharacterContainer.container == null)
            return null;

        var position = characterContainers.Count;

        if (item.CharacterData.model == null)
        {
            Debug.LogWarning("Character's model is empty, this MUST be set");
            return null;
        }

        var container = helperCharacterContainer.container;
        container.RemoveAllChildren();

        var character = Instantiate(item.CharacterData.model);
        character.SetFormation(this, position, helperCharacterContainer.priority, container);
        character.Item = item;
        CharacterBuffComponent buffComp = character.gameObject.AddComponent<CharacterBuffComponent>();
        buffComp.buffs = helperCharacterContainer.buffs;
        Characters[position] = character;

        return character;
    }

    public virtual void ClearCharacters()
    {
        foreach (var container in characterContainers)
        {
            container.container.RemoveAllChildren();
        }
        if (helperCharacterContainer.container != null)
            helperCharacterContainer.container.RemoveAllChildren();
        Characters.Clear();
    }
}
