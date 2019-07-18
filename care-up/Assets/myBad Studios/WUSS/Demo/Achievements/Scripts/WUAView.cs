using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MBS {
	public class WUAView : MonoBehaviour {
        public CMLData Fields { get; set; } = null;
        /*
        Fields contain:
        int aid
        string name
        string text //unlocked text
        string descr //unlocking instructions
        string locked_url //path to icon if not in a Resources folder
        string unlocked_url //path to icon if not in a Resources folder
        string requirements //a comma delimited array of requirement strings in the format: TYPE NAME QTY
        */

        [SerializeField] Image icon = default(Image);
        [SerializeField] Text name = default(Text);
        [SerializeField] Text description = default(Text);

        Sprite
            LockedImg,
            UnlockedImg;

        bool initialized = false;
        public bool Unlocked { get { return Fields.Bool( "unlocked" ); } set { Fields.Seti( "unlocked", value ? 1 : 0 ); } }

        void Start() => Initialize();

        public void Initialize()
        {
            if ( initialized )
                return;

            initialized = true;
            Fields.obj = this;
            name.text = Fields.String("name");

            //see if this sprite's image is found inside the project and if so, load that.
            //if not found locally, download it from the web
            //LockedImg = Resources.Load<Sprite>( ResourceFilename( Fields.String( "locked_url" ) ) );
            //UnlockedImg = Resources.Load<Sprite>( ResourceFilename( Fields.String( "unlocked_url" ) ) );

            LockedImg = Resources.Load<Sprite>("Sprites/btn_icon_lock");
            UnlockedImg = Resources.Load<Sprite>("Sprites/cup");

            DisplayRelevantVersion();

            /*if (GameObject.Find("Account_Achievements"))
            {
                if (null == LockedImg)
                    StartCoroutine(FetchImageOnline(Fields.String("locked_url"), true));
                if (null == UnlockedImg)
                    StartCoroutine(FetchImageOnline(Fields.String("unlocked_url"), false));
            } */          
        }

        public void DisplayRelevantVersion()
        {
            //description.text = Unlocked ? Fields.String( "text" ) : Fields.String( "descr" );
            description.text = Fields.String("text"); // just display text
            icon.sprite = Unlocked ? UnlockedImg : LockedImg;
        }

        IEnumerator FetchImageOnline(string url, bool locked_sprite)
        {
#pragma warning disable
            WWW w = new WWW( url );
#pragma warning restore
            yield return w;
            if ( string.IsNullOrEmpty( w.error ) )
            {
                if ( w.texture )
                {
                    Texture2D tex = null;
                    tex = w.texture;
                    Rect size = new Rect( 0, 0, tex.width, tex.height );
                    Vector2 pivot = new Vector2( 0.5f, 0.5f );
                    if ( locked_sprite )
                        LockedImg = Sprite.Create( tex, size, pivot );
                    else
                        UnlockedImg = Sprite.Create( tex, size, pivot );
                }

                //see if the currently downloaded image should be displayed
                if ( locked_sprite && !Unlocked )
                    icon.sprite = LockedImg;
                if ( !locked_sprite && Unlocked )
                    icon.sprite = UnlockedImg;
            }
        }

        //extract the filename from the URL so we can see if the image exists inside the project
        string ResourceFilename( string s )
        {
            if ( s.LastIndexOf( "." ) == s.Length - 4 )
                s = s.Substring( 0, s.Length - 4 );
            if ( s.LastIndexOf( '/' ) >= 0 )
                s = s.Substring( s.LastIndexOf( '/' ) + 1 );

            return s;
        }

    }
}