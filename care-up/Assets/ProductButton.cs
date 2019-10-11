using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    public Text _name;
    public GameObject price;
    public Text cost;
    public GameObject checkmark;
    public GameObject dressOn;

    public Image icon;
    public StoreItem item;
    TabGroup tabGroup;

    void Start()
    {
        dressOn.SetActive(false);
    }
    
    public void Select(bool toSelect)
    {
        if (toSelect)
        {
            GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }

    public void ButtonClicked()
    {
        tabGroup.SelectItem(this);
    }

    public void Initialize(StoreItem _item, TabGroup tg)
    {
        item = _item;
        _name.text = item.name;
        SetPrice(item.price);
        SetPurchased(item.purchased);
        tabGroup = tg;
        Sprite sprite = Resources.Load("Sprites/StoreItemPreview/" + item.name, typeof(Sprite)) as Sprite;
        if (sprite != null)
        {
            icon.sprite = sprite;
        }
    }

    public void SetPrice(int _price)
    {
        cost.text = _price.ToString();
    }

    public void SetPurchased(bool isPurchased)
    {
        price.SetActive(!isPurchased);
        checkmark.SetActive(isPurchased);
    }

    public void SetDressOn(bool isDressedOn)
    {
        dressOn.SetActive(isDressedOn);
    }
}
