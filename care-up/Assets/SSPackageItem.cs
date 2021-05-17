using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSPackageItem : MonoBehaviour
{
    public Text titleText;
    public Text priceText;
    public float price;
    public string packageSKU = "";
    public void Setup(string _title, List<string> _scenes, string _packageSKU, float _price)
    {
        price = _price;
        priceText.text = "€" + price.ToString("F2");
        Object SceneItemPrefab = Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageItemScene");
        titleText.text = _title;
        packageSKU = _packageSKU;
        Transform sceneSlot = transform.Find("SSNames");
        GameObject spacer1 = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageSpacer"), sceneSlot);
        Vector2 spacer1Size = spacer1.GetComponent<RectTransform>().sizeDelta;
        spacer1Size.y = 10f;
        spacer1.GetComponent<RectTransform>().sizeDelta = spacer1Size;

        if (_scenes != null)
        {
            for (int i = 0; i < _scenes.Count; i++)
            {
                GameObject SceneItemObject = Instantiate(SceneItemPrefab, sceneSlot) as GameObject;
                SceneItemObject.transform.Find("Text").GetComponent<Text>().text = "• " + _scenes[i];
            }
            GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageSpacer"), sceneSlot);
        }
    }

    public void ButtonPressed()
    {

    }
}
