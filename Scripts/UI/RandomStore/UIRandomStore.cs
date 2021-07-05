using UnityEngine;
using UnityEngine.UI;

public class UIRandomStore : UIDataItemList<UIRandomStoreItem, RandomStoreItem>
{
    public Text textEndsIn;
    public RandomStore randomStore;
    public AnimItemsRewarding animItemsRewarding;
    public RandomStoreEvent StoreEvent { get; private set; }
    private float refreshCountDown;

    private void OnEnable()
    {
        GetRandomStore();
    }

    protected override void Update()
    {
        base.Update();
        if (refreshCountDown > 0)
            refreshCountDown -= Time.unscaledDeltaTime;
        if (refreshCountDown < 0)
            refreshCountDown = 0;
        if (textEndsIn != null)
        {
            textEndsIn.text = refreshCountDown.ToString("N0");
        }
    }

    private void GetRandomStore()
    {
        ClearListItems();
        GameInstance.GameService.GetRandomStore(randomStore.Id, OnGetRandomStoreSuccess, OnGetRandomStoreFail);
    }

    private void OnGetRandomStoreSuccess(RandomStoreResult result)
    {
        StoreEvent = result.store;
        refreshCountDown = result.endsIn;
        for (var i = 0; i < result.store.RandomedItems.Count; ++i)
        {
            var entry = result.store.RandomedItems[i];
            var ui = SetListItem(i.ToString());
            ui.data = entry;
            ui.uiRandomStore = this;
            ui.index = i;
            ui.Show();
        }
    }

    private void OnGetRandomStoreFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error, GetRandomStore);
    }
}
