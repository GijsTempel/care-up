using UnityEngine;
using UnityEngine.UI; 
using MBS;

//This script demonstrates how you can pop the login prefab back into view again from your own code
//and make it show any menu you like...
public class DemoObjectUGUI : MonoBehaviour {

    public WUUGLoginGUI login;
    public Image avatar;
    public Text player_name_text;

    void Start()
    {
        avatar?.gameObject.SetActive( false );
        player_name_text?.gameObject.SetActive( false );

        WULogin.onGravatarTextureFetched += OnTextureFetched;
        WULogin.onGravatarSpriteFetched += OnSpriteFetched;
        WULogin.onLoggedIn += FindLogin;
        WULogin.onLoggedOut += Cleanup;
        WULogin.onResumeGame += OnResumeGame;
    }

    void OnResumeGame()
    {
        if ( null != avatar.sprite )
        {
            avatar.gameObject.SetActive( true );
            player_name_text.gameObject.SetActive( true );
        }
    }

    void OnTextureFetched( Texture2D tex ) => Debug.LogWarning( $"Tex has size {tex.width}");

	void OnSpriteFetched(Sprite sprite)
	{
		if (null != avatar)
		{
			avatar.sprite = sprite;
			avatar.rectTransform.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
		}

		if (null != player_name_text)
		{
			player_name_text.rectTransform.sizeDelta = new Vector2(sprite.texture.width, 30f);
			player_name_text.text = WULogin.nickname;
		}

#if WUSKU
        if ( WULogin.RequireSerialForLogin && !WULogin.HasSerial )
            return;
#endif

        player_name_text?.gameObject.SetActive( true );
        avatar?.gameObject.SetActive( true );
    }

    public void ShowPostLoginMenu()
	{
		if (!WULogin.logged_in || login.active_state != WUUGLoginGUI.eWULUGUIState.Inactive)
			return;
		#if WUS
		Debug.LogWarning (WULogin.highscore);
		#endif
		login.gameObject.SetActive(true);
		login.ShowPostLoginMenu();
        avatar.gameObject.SetActive( false );
        player_name_text.gameObject.SetActive( false );
	}

	void FindLogin(CML response)
	{
		if (null == login)
			login = FindObjectOfType<WUUGLoginGUI>();
	}

	void Cleanup(CML response)
	{
		avatar?.gameObject.SetActive(false);
		player_name_text?.gameObject.SetActive(false);
	}

}
