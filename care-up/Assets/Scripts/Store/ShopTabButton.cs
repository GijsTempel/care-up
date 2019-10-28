using UnityEngine;

public class ShopTabButton : MonoBehaviour
{
    public GameObject SelectedImage;
    public GameObject UnselectedImage;
    
    public bool TabState = false;
    // Start is called before the first frame update

    public void SetState(bool value)
    {
       SelectedImage.SetActive(value);
       UnselectedImage.SetActive(!value); 
    }   
}
