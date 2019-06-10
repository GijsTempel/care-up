using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBS {
    /// <summary>
    /// If you have the WordPress Data asset then you can store your keys online rather than locally
    /// When storing it locally an achievement is unlocked on all devices once you achive it on one device
    /// When storing it online, all devices contribute to unlocking an achievement. For example:
    /// If you have to kill 100 enemies and you store keys offline then you have to kill 100 on one device
    /// but if you store it online then all kills on all devices count towards your achivement goal
    /// </summary>
    public class WUAchieveManager : MonoBehaviour {
#if WUDATA
        static WUAchieveManager _instance;
        static public WUAchieveManager Instance
        {
            get
            {
                if ( null == _instance )
                {
                    GameObject go = new GameObject( "AchievementManager" );
                    _instance = go.AddComponent<WUAchieveManager>(); 
                }
                return _instance;
            }
        }

        public CML Keys;
        CMLData _keys => Keys [0];

        //store all the award info we received from the server
        public CML all_awards;

        //we only try to auto award the achievements with requirements set on server side
        public List<CMLData> tracked;

        void Start()
        {
            //if _instance is set then Start already ran once so quit.
            if ( null != _instance )
            {
                //but if this is a duplicate instance then destroy it
                if ( this != _instance )
                    Destroy( gameObject );
                return;
            }

            //if it's not set then this is the first script that is trying to use it so set this as _instance
            if ( null == _instance )
                _instance = this;

            DontDestroyOnLoad( gameObject );

            //wait until login was successful then download all keys
            //this is great during the demo but if you spawn your prefab(s) mid game
            //you might have to call it manually. Either way, see the FetchAwards function to see how
            WULogin.onLoggedIn += FetchAwards;
            WULogin.onLoggedIn += FetchKeys;
            WULogin.onLoggedOut += ClearEverything;
            if ( null == Keys )
                Keys = new CML();
        }

        void ClearEverything( CML response )
        {
            Keys = null;
            all_awards = null;
            tracked = null;
        }

#region awards
        void FetchAwards( CML response ) => WUAchieve.FetchEverything( GenerateEntries );
        void GenerateEntries( CML response )
        {
            //store the server results then extract the achievements to work with in this function
            all_awards = response;
            List<CMLData> entries = all_awards.Children( 0 );
            tracked = new List<CMLData>();
            foreach ( CMLData entry in entries )
            {
                //if an entry has server side requirements AND hasn't been unlocked already, track it for auto unlocking
                if ( entry.String( "requirements" ).Trim() != "" && !entry.Bool( "unlocked" ) )
                    tracked.Add( entry );
            }
        }

        public void AwardAchievement( int aid )
        {
            List<CMLData> entries = all_awards.Children( 0 );
            foreach ( CMLData entry in entries )
                if ( entry.Int( "aid" ) == aid )
                    _displayAchievementNotification( entry );
            WUAchieve.UnlockAchievement( aid );
        }
#endregion

        public void ScanUnlockedStatus()
        {
            bool achieved = true;
            for ( int i = tracked.Count-1; i >= 0; i-- )
            {
                //assuming that the test will pass makes it easier to determine when one of the series of requirements caused the entire test to fail
                achieved = true;

                //requirements are sent as a comma delimited array so first step is to separate the various requirements
                string [] requirements = tracked[i].String( "requirements" ).Split( ',' );

                //next we test each requirement in turn to see if all of them past their respective tests...
                foreach ( string requirement in requirements )
                {
                    //if one of the requirements failed then there is no point in continuing to test the rest. Move on to the next achievement
                    if ( !achieved )
                        continue;

                    string [] elements = requirement.Split( ' ' );
                    if ( elements.Length < 3 || elements [2].Trim() == "" )
                    {
                        achieved = false;
                        UnityEngine.Assertions.Assert.IsFalse( achieved, $"{requirement} is not a properly formatted requirement" );
                        continue;
                    }

                    //requirements have the format: TEST NAME QTY. Example GT Gold 500. 

                    //let's fetch the current value of the key
                    int value = _keys.Int( elements [1] );

                    //Get a numeric 3rd element
                    int qty = int.Parse( elements [2] );

                    //and see if the current test fails. By default we assume it passed so we only need to check if we are wrong
                    switch ( elements [0] )
                    {
                        case "LT":
                            if ( value >= qty )
                                achieved = false;
                            break;

                        case "GT":
                            if ( value < qty )
                                achieved = false;
                            break;

                        case "EQ":
                            if ( value != qty )
                                achieved = false;
                            break;
                    }
                }

                //if achieved is still true at this point then an achievement needs to be unlocked!
                if ( achieved )
                {
                    WUAchieve.UnlockAchievement( tracked [i].Int( "aid" ) );
                    _displayAchievementNotification( tracked [i] );
                }
            }
        }

        void _displayAchievementNotification( CMLData achievement )
        {
            //don't display duplicate awarding of the same award
            if ( achievement.Bool( "unlocked" ) )
                return;

            achievement.Set( "unlocked", "true" );

            if ( tracked.Contains( achievement ) )
                tracked.Remove( achievement );

            StartCoroutine(FetchImageOnline(achievement.String("unlocked_url"), achievement.String("name")) );
        }

        //extract the filename from the URL so we can see if the image exists inside the project
        string ResourceFilename( string s ) => s.Substring( s.LastIndexOf( '/' ), s.Length - s.LastIndexOf( '/' ) - 4);

        IEnumerator FetchImageOnline( string url, string message )
        {
            string filename = ResourceFilename( url );
            Sprite sprite = Resources.Load<Sprite>( filename );
            if ( null == sprite )
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
                        sprite = Sprite.Create( tex, size, pivot );
                    }
                }
            }

            Canvas c = FindObjectOfType<Canvas>();
            GameObject hc = GameObject.Find( "HUDCanvas" );
            yield return new WaitForSeconds( 1f );
            if ( null != c )
                MBSNotification.SpawnInstance( null == hc ? c : hc.GetComponent<Canvas>(), new Vector2( 230f, -175f ), new Vector2( -20f, -175f ), "Achievment unlocked", message, sprite );
        }

        #region keys
        void SaveKeys() => WUData.UpdateCategory( "achievement_keys", _keys.Copy( CMLCopyMode.no_id ) );
        void FetchKeys( CML response ) => WUData.FetchCategory( "achievement_keys", _parseKeys, WPServer.GameID, _createKeys );
        void _parseKeys( CML response ) {
            //fetch the keys online and remove the header node that WUData inserts at position 0
            Keys = response;
            if ( Keys.Count < 2 )
                _createKeys( null );
            else
                Keys.RemoveNode( 0 );
        }

        //if this category was not found then create it locally
        void _createKeys( CMLData ignore )
        {
            Keys = new CML();
            Keys.AddNode( "Keys" );
        }

        public void UpdateKeys( string name, int qty = 0 )
        {
            //Save the current tracking keys so we are up to date across game sessions
            _keys.Add( qty, name );
            SaveKeys();

            //since the keys have been updated, let's see if anything is now unlocked
            ScanUnlockedStatus();
        }
        #endregion

#endif
    }
}
