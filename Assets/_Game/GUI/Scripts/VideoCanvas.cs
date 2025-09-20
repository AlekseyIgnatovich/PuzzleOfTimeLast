using UnityEngine;

public class VideoCanvas : MonoBehaviour {

    [SerializeField] ItemsData itemsData;
    [SerializeField] HeroBase heroBase;
    [SerializeField] Canvas canvas;
    [Space]
    [SerializeField] LotteryRewardPanel lotteryPanel;
    [SerializeField] ShopHeroReward heroRewardPanel;

    [HideInInspector] public ShopItem item;

    public void VideoEnd() {
        canvas.sortingOrder = 0;

        if (item == null) { return; }

        ShopPlayItem spi;
        ShopHeroItem shi;
        ShopLotteryItem sli;

        spi = item as ShopPlayItem;
        if (spi != null) {
            itemsData.AddItem(spi.item);
            return;
        }
        shi = item as ShopHeroItem;
        if (shi != null) {
            heroBase.AddCardToInventory(shi.GetCard());
            heroRewardPanel.Setup(shi.GetCard());
            return;
        }
        sli = item as ShopLotteryItem;
        if (sli != null) {
            lotteryPanel.Open(sli.item);
            return;
        }
    }

    public void OpenVideo(ShopItem _item) {
        item = _item;
        canvas.sortingOrder = 100;
        gameObject.SetActive(true);
    }
    public void CloseVideo() {
        VideoEnd();
        gameObject.SetActive(false);
    }
}
