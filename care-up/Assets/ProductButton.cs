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
    Color originalColor = new Color();

    void Start()
    {
        //originalColor = GetComponent<Image>().color;
        // dressOn.SetActive(false);
        _name.gameObject.SetActive(false);
    }
    
    public void Select(bool toSelect)
    {
        //if (toSelect)
        //{
        //    GetComponent<Image>().color = new Color(0f, 214f, 255f, 255f);
        //}
        //else
        //{
        //    GetComponent<Image>().color = originalColor;
        //}
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
        if (item.name == "x")
        {
            checkmark.SetActive(false);
            sprite = Resources.Load("Sprites/StoreItemPreview/x", typeof(Sprite)) as Sprite;
        }
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
        item.purchased = isPurchased;
        price.SetActive(!isPurchased);
        checkmark.SetActive(isPurchased);
    }

    public void SetDressOn(bool isDressedOn)
    {
        dressOn.SetActive(isDressedOn);
    }
}
