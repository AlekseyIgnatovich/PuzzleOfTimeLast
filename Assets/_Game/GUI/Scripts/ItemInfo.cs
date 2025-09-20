using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{

    [SerializeField] GameManager manager;
    [SerializeField] ItemsData itemData;
    [Space]
    [SerializeField] LootboxRewardPanel rewardPanel;
    [Space]
    [SerializeField] GameObject panel;
    [SerializeField] GameObject openButton;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textDescription;
    [SerializeField] TextMeshProUGUI textAmount;

    private void OnEnable()
    {
        itemData.OnSelectItem += SetInfo;
    }
    private void OnDisable()
    {
        itemData.OnSelectItem -= SetInfo;
    }

    public void SetInfo()
    {
        Debug.Log("Opening item info");
        ItemObject _itm = itemData.selectedItem;

        if (_itm == null) { return; }

        panel.SetActive(true);
        if (manager.state == GameState.Items)
        {
            openButton.SetActive(_itm.item.targetType == ItemType.Inventory);
        }
        else
        {
            openButton.SetActive(false);
        }

        image.sprite = _itm.item.sprite;
        textName.text = _itm.item.Name;
        textDescription.text = _itm.item.effect.description;
        textAmount.text = $"{_itm.amount}";
    }
    public void CloseInfo()
    {
        Debug.Log("Cerrando ventana de item");
        panel.SetActive(false);
        openButton.SetActive(false);
    }

    public void OpenLootboxItem()
    {
        //if (itemData.selectedItem.amount == 0) return;
        if (itemData.selectedItem == null) { return; }
        if (itemData.selectedItem.item.targetType == ItemType.Inventory)
        {


            int _ind = itemData.SelectedItemIndex();
            if ((_ind >= 0) && !itemData.items[_ind].SubtractAmount())
            {
                if (_ind >= 0)
                {
                    itemData.items[_ind].selected = false;
                    itemData.items[_ind] = null;
                    itemData.Save();
                    FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
                }
            }
            Instantiate(rewardPanel, manager.canvas.transform).Setup(itemData.selectedItem.item.effect, manager);
        }
        CloseInfo();
    }
}
