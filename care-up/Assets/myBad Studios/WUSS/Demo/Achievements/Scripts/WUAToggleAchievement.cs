using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using MBS;

/// <summary>
/// This class merely spawns a button inside a grid and when clicked calls ToggleAchievement on the demo prefab
/// </summary>
public class WUAToggleAchievement : MonoBehaviour, IPointerClickHandler {

    static public WUAToggleAchievement SpawnInstance(int aid, WUADisplay grandparent, Transform parent)
    {
        WUAToggleAchievement result = Instantiate(Resources.Load<WUAToggleAchievement>("ToggleIcon"));
        result.transform.SetParent( parent, false );
        result.panel_prefab = grandparent;
        result.aid = aid;
        return result;
    }

    [SerializeField] Image icon = default(Image);
    WUADisplay panel_prefab;
    int aid;

    public void SetIcon( Sprite to ) => icon.sprite = to;
    public void OnPointerClick( PointerEventData data ) => panel_prefab?.ToggleAchievement( aid );
}
