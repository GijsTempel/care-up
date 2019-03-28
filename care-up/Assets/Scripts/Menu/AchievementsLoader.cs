using UnityEngine;

public class AchievementsLoader : MonoBehaviour
{
    public GameObject accountAchievementsPanel;
    public GameObject loadingIcon;
    public GameObject backButton;
    public GameObject image;
    public GameObject title;
    public GameObject separator;

    public void Update()
    {
        if (accountAchievementsPanel.activeInHierarchy == false && loadingIcon.activeInHierarchy)
            loadingIcon.SetActive(false);
    }
}

