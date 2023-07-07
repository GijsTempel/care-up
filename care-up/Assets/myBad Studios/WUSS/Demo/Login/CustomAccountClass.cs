using UnityEngine;
using MBS;

/// <summary>
/// 
/// Notice how this demo class has ONLY private functions
/// 
/// This script demonstrates that you need never modify the WUUGLoginGUI class as it is merely a cosmetic front end
/// The underlaying WULogin sends out events that you can plug into and that way link your code with
/// the login kit... Conversely, since the WULogin class does all the work it means that WUUGLoginGUI is entirely optional
/// and can be customised or replaced entirely, should you want or need to do so
/// 
/// Additionally, this demo scene will check to see if you have a game called "Demo game" created on your website
/// IF it doesn't find that game it will create it and update your local list of the games you have on your website
/// </summary>

public class CustomAccountClass : MonoBehaviour {

#if UNITY_EDITOR
    bool first_run_completed = false;

	void Start()
	{
		WULogin.onLoggedIn += OnLoggedIn;
		WULogin.onLoggedOut += OnLoggedOut;
		WULogin.onLoginFailed += OnLoginFail;
        WULogin.onGameListFetched += OnGameListFetched;
        WULogin.onLoggedIn += DoCreateDemoGame;
        WULogin.onResumeGame += OnResumeGame;
	}
    void OnDestroy() => WULogin.onGameListFetched -= OnGameListFetched;

    void OnGameListFetched( CML data ) => Debug.Log( data.ToString() );
	void OnLoginFail(CMLData response) => Debug.Log( $"Error message from failed login: {response.String("message")}" );
    void OnLoggedIn( CML data ) => Debug.Log( $"Yeah! Logged in! Now I can load my level! ID:{WPServer.GameID}" );
	void OnLoggedOut(CML data) => Debug.Log( "Oh, no! End of line! Time to load the main menu scene again" );

    void OnResumeGame()
    {
        if ( !first_run_completed )
            DoCreateDemoGame( null );
    }

    void DoCreateDemoGame(CML response)
    {
#if WUSKU

        if ( WULogin.RequireSerialForLogin && !WULogin.HasSerial )
            return;
#endif

        //only do all of this once
        if ( first_run_completed )
            return;

        if (null == WULogin.AvailableGames)
            WULogin.LoadAvailableGames();

        // if you have no game definitions stored locally go fetch them from your website
        if ( WULogin.AvailableGames.Count < 2 )
        {
            //first define what to do with the server responses...
            WULogin.onGameListFetched += ScanCreatedGames;
            WULogin.onGameListFetchFailed += CouldntFetchGames;

            //...then call the server!
            WULogin.FetchAvailableGameInfo();
        }
        else
            ScanCreatedGames( null ) ;

        first_run_completed = true;
    }

    //look at the game definitions stored locally to see if "Demo Game" is among them. If so, report it's ID
    void ScanCreatedGames( CML response )
    {
        int game_id = 0;

        //since we are now in this function we no longer need to listen to the event which sends us here
        WULogin.onGameListFetched -= ScanCreatedGames;
        WULogin.onGameListFetchFailed -= CouldntFetchGames;

        //look for the game definition
        foreach ( CMLData entry in WULogin.AvailableGames )
        {
            if ( entry.String( "name" ) == "Demo Game" )
                game_id = entry.Int( "gid" );
        }

        //if not found, create it
        if ( game_id == 0 )
        {
            //first define what to do with the server responses...
            WULogin.onNewGameCreated += AfterCreatingDemoGame;
            WULogin.onNewGameCreationFailed += AfterFailingToCreateDemoGame;

            //...then call the server!
            WULogin.CreateNewGame( "Demo Game", "A game used for feature demonstration purposes", "Demo game" );
        }
        else
            Debug.LogWarning($"Demo Game has ID {game_id}");
    }

    void AfterCreatingDemoGame( CML response )
    {
        //since we are now in this function we no longer need to listen to the event which sends us here
        WULogin.onNewGameCreated -= AfterCreatingDemoGame;
        WULogin.onNewGameCreationFailed -= AfterFailingToCreateDemoGame;
        int game_id = 0;
        foreach ( CMLData entry in WULogin.AvailableGames )
        {
            if ( entry.String( "name" ) == "Demo Game" )
                game_id = entry.Int( "gid" );
        }

        Debug.Log(game_id == 0 ? "Something went wrong! The game was not created!" : $"Created Demo Game with ID {game_id}");
    }

    void AfterFailingToCreateDemoGame( CMLData response )
    {
        //since we are now in this function we no longer need to listen to the event which sends us here
        WULogin.onNewGameCreated -= AfterCreatingDemoGame;
        WULogin.onNewGameCreationFailed -= AfterFailingToCreateDemoGame;

        print( response.String( "message" ) );
    }

    void CouldntFetchGames( CMLData response )
    {
        //since we are now in this function we no longer need to listen to the event which sends us here
        WULogin.onGameListFetched -= ScanCreatedGames;
        WULogin.onGameListFetchFailed -= CouldntFetchGames;

        print( response.String( "message" ) );
    }
#endif 
}
