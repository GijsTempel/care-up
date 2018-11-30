using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace MBS
{
    [CustomEditor( typeof( WPServer ) )]
    public class WPServerEditor : Editor
    {
        string[] gamesList;
        int index, last_index;

        WPServer s;
        SerializedProperty serializedGameID;

        void PopulateGameList()
        {
            if ( WULogin.AvailableGames.Count < 2 )
            {
                index = 0;
                gamesList = new string [] { "No Games Found" };
            }
            else
            {
                if ( null == gamesList || gamesList.Length != WULogin.AvailableGames.Count - 1 )
                {
                    gamesList = new string [WULogin.AvailableGames.Count - 1];
                    int i = 0;
                    for ( i = 0; i < gamesList.Length; i++ )
                        gamesList [i] = WULogin.AvailableGames [i + 1].String( "name" );
                }
            }
        }

        void OnEnable()
        {
            if ( null == WULogin.AvailableGames )
                WULogin.LoadAvailableGames();

            s = (WPServer)target;
            serializedGameID = serializedObject.FindProperty( "game_id" );

            int i = index = last_index = 0;
            if ( WULogin.AvailableGames.Count > 1 )
            {
                for ( i = 0; i < WULogin.AvailableGames.Count - 1; i++ )
                {
                    if ( WULogin.AvailableGames [i + 1].Int( "gid" ) == WPServer.GameID )
                        index = i;
                }
                SetNewGameID( WULogin.AvailableGames [index + 1].Int( "gid" ) );
            }
            else
                SetNewGameID( 1 );
        }

        void SetNewGameID(int value)
        {
            serializedGameID.intValue = value; 
            EditorUtility.SetDirty( s.gameObject );
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            PopulateGameList();

            if ( null == s )
                return;

            if ( gamesList != null && !Application.isPlaying )
                index = EditorGUILayout.Popup( "Select Game", index, gamesList );
            if ( last_index != index )
            {
                last_index = index;
                SetNewGameID( WULogin.AvailableGames [index + 1].Int( "gid" ) );
            }

            if ( GUILayout.Button( "Refresh Games List" ) && !Application.isPlaying )
                WULogin.FetchAvailableGameInfo();
            
            EditorGUILayout.LabelField( "Game ID", $"{serializedGameID.intValue}" );
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
}