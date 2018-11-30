using UnityEngine;
using UnityEngine.UI;

public class WULocalizationButton : MonoBehaviour {

    [SerializeField] int ID;
    [SerializeField] Image graphic;
    [SerializeField] Image blocker;

    public void SetId( int to, Sprite avatar)
    {
        ID = to;
        graphic.sprite = avatar;
    }

    public void SelectALanguage( int which ) => blocker.gameObject.SetActive( which == ID );
    public void SelectALanguage(WULocalizationButton which)
    {
        if ( which == this )
            SendMessageUpwards( "ConsiderNewLocalisation", transform.GetSiblingIndex() );
    }

}
