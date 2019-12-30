using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProductButton : MonoBehaviour
{
    [SerializeField]
    private GameObject checkmark = default,
                       dressOn = default,
                       price = default,
                       coinMark = default,
                       diamondMark = default;

    [SerializeField]
    private Text productName = default,
                      cost = default;

    [SerializeField]
    private Image icon = default;

    private TabGroup tabGroup;
    private Color originalColor = new Color();
    private Image image;

    Sprite nextIcon = null;
    public StoreItem item;

    void Start()
    {
        image = transform.Find("ProductPanel/Image").GetComponent<Image>();
        originalColor = image.color;
        dressOn.SetActive(false);
        productName.gameObject.SetActive(false);

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        Button_Functions bf = GameObject.FindObjectOfType<Button_Functions>();

        entry.callback.AddListener((eventData) => { bf.OnButtonHover(); });
        trigger.triggers.Add(entry);

        transform.Find("ProductPanel").GetComponent<Button>().onClick.AddListener(() => bf.OnButtonClick());
    }

    public void Select(bool toSelect)
    {
        image.color = toSelect ? new Color(0f, 214f, 255f, 255f) : originalColor;
    }

    public void ButtonClicked()
    {
        tabGroup.SelectItem(this);
    }

    public void Initialize(StoreItem _item, TabGroup tg)
    {
        item = _item;
        productName.text = item.name;
        SetCurrancySprite(item.extraPrice);

        if (item.extraPrice > 0)
            SetPrice(item.extraPrice);
        else
            SetPrice(item.price);

        SetPurchased(item.purchased);
        tabGroup = tg;
        nextIcon = Resources.Load("Sprites/StoreItemPreview/" + item.name, typeof(Sprite)) as Sprite;
        Invoke("loadSprite", 0.1f);

        if (item.name == "x")
        {
            checkmark.SetActive(false);
            nextIcon = Resources.Load("Sprites/StoreItemPreview/x", typeof(Sprite)) as Sprite;
        }
    }

    void loadSprite()
    {
        if (nextIcon != null)
            icon.GetComponent<Image>().sprite = nextIcon;
    }

    public void SetPrice(int price)
    {
        cost.text = price.ToString();
    }

    private void SetCurrancySprite(int extraPrice)
    {
        if (extraPrice > 0)
        {
            diamondMark.SetActive(true);
            coinMark.SetActive(false);
        }
        else
        {
            diamondMark.SetActive(false);
            coinMark.SetActive(true);
        }
    }

    public void SetPurchased(bool isPurchased)
    {
        item.purchased = isPurchased;
        price.SetActive(!isPurchased);
        checkmark.SetActive(isPurchased);
    }

    public void SetDressOn(bool isDressedOn)
    {
        dressOn.SetActive(isDressedOn);
    }
}
