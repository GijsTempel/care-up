using UnityEngine;
using MBS;

/// <summary>
/// This demo will systematically run through every single function in this kit
/// creating sample data, fetching back that data in the form of a field, category or everything
/// then removing it again and leaving us with noo trace in the database that this demo even run
/// This demo manipulates your own data, shared data / game settings as well as other people's data
/// </summary>
public class WUDataDemo3 : MonoBehaviour {

    [SerializeField] int ID_of_someone_else = 999;
    [SerializeField] bool WUData_pro = false;

    CMLData demo_data;

    int fictional_game_id = 999;
    int GID => fictional_game_id;

    void Start() => WULogin.onLoggedIn += RunDemo;

    void RunDemo( CML ignore )
    {
        WUData.WUDataPro = WUData_pro;

        //first let's create some data
        demo_data = new CMLData();
        demo_data.Set( "Foo", "bar" );
        demo_data.Set( "Marco", "Polo" );
        demo_data.Set( "Hallo", "world" );
        demo_data.Set( "superhero", "Wonder Woman" );

        //lets start by saving some sample data to two categories under our own account for fictional game id 999
        WUData.UpdateCategory( "", demo_data, GID );
        WUData.UpdateCategory( "demo", demo_data, GID, FetchAField, PrintError );
    }

    void FetchAField( CML response )
    {
        WUData.FetchField( "superhero", "demo", FetchACategory, GID, PrintError );
    }

    void FetchACategory( CML response )
    {
        PrintResponse( response );
        WUData.FetchCategory( "demo", FetchCategoryLike, GID, PrintError );
    }

    void FetchCategoryLike( CML response )
    {
        PrintResponse( response );
        WUData.FetchCategoryLike( "em", FetchAllCategories, GID, PrintError );
    }

    void FetchAllCategories( CML response )
    {
        PrintResponse( response );
        WUData.FetchGameInfo( FetchUncatgorised, GID, PrintError );
    }

    void FetchUncatgorised( CML response )
    {
        PrintResponse( response );
        WUData.FetchGlobalInfo( RemoveAField, PrintError );
    }

    void RemoveAField( CML response )
    {
        PrintResponse( response );
        WUData.RemoveField( "Marco", "demo", RemoveACategory, GID, PrintError );
    }

    void RemoveACategory( CML response )
    {
        print( "Removed Marco" );
        WUData.RemoveCategory( "", FetchEVERYTHING, GID, PrintError );
    }

    void FetchEVERYTHING( CML response )
    {
        print( "Removed Category" );
        WUData.FetchEverything( DeleteEverything, PrintError );
    }

    void DeleteEverything( CML response )
    {
        PrintResponse( response );
        WUData.RemoveGameData( GID, RunSharedDemo, PrintError );
    }

    //------------------------------------------------------------

    void RunSharedDemo( CML ignore )
    {
        //first let's create some data
        demo_data = new CMLData();
        demo_data.Set( "FooShared", "bar" );
        demo_data.Set( "MarcoShared", "Polo" );
        demo_data.Set( "HalloShared", "world" );
        demo_data.Set( "superheroShared", "Wonder Woman" );

        //lets start by saving some sample data to two categories under our own account for fictional game id 999
        //also, let's create some global settings for this user
        WUData.UpdateSharedCategory( "", demo_data, GID );
        WUData.UpdateSharedCategory( "demo", demo_data, GID, FetchASharedField, PrintError );

        demo_data.Clear();
        demo_data.Setf( "Volume", 0.5f );
        WUData.UpdateSharedCategory( "Settings", demo_data, 0 );
        print("SHARED DATA");
    }

    void FetchASharedField( CML response )
    {
        WUData.FetchSharedField( "superheroShared", "demo", FetchASharedCategory, GID, PrintError );
    }

    void FetchASharedCategory( CML response )
    {
        PrintResponse( response );
        WUData.FetchSharedCategory( "demo", FetchSharedCategoryLike, GID, PrintError );
    }

    void FetchSharedCategoryLike( CML response )
    {
        PrintResponse( response );
        WUData.FetchSharedCategoryLike( "em", FetchAllSharedCategories, GID, PrintError );
    }

    void FetchAllSharedCategories( CML response )
    {
        PrintResponse( response );
        WUData.FetchAllSharedInfo( RemoveASharedField, GID, PrintError );
    }

    void RemoveASharedField( CML response )
    {
        PrintResponse( response );
        WUData.RemoveSharedField( "MarcoShared", "demo", RemoveASharedCategory, GID, PrintError );
    }

    void RemoveASharedCategory( CML response )
    {
        print( "Removed a field" );
        WUData.RemoveSharedCategory( "demo", gid:GID );
        WUData.RemoveSharedCategory( "", RunUserDemo, GID, PrintError );
    }

    //-----------------------------------------------------------

    void RunUserDemo( CML ignore )
    {
        //first let's create some data
        demo_data = new CMLData();
        demo_data.Set( "Foo", "bar" );
        demo_data.Set( "Marco", "Polo" );
        demo_data.Set( "Hallo", "world" );
        demo_data.Set( "superhero", "Wonder Woman" );

        //lets start by saving some sample data to two categories under our own account for fictional game id 999
        WUData.UpdateUserCategory( ID_of_someone_else, "", demo_data, GID );
        WUData.UpdateUserCategory( ID_of_someone_else, "demo", demo_data, GID, FetchAUserField, PrintError );

        demo_data.Clear();
        demo_data.Set( "My name", "John Doe" );
        demo_data.Set( "My valentine", "Rosie McDonall" );
        WUData.UpdateUserCategory(ID_of_someone_else, "Personal", demo_data, 0 );
        print( "USER DATA" );
    }

    void FetchAUserField( CML response )
    {
        WUData.FetchUserField( ID_of_someone_else, "superhero", "demo", FetchAUserCategory, GID, PrintError );
    }

    void FetchAUserCategory( CML response )
    {
        PrintResponse( response );
        WUData.FetchUserCategory( ID_of_someone_else, "demo", FetchUserCategoryLike, GID, PrintError );
    }

    void FetchUserCategoryLike( CML response )
    {
        PrintResponse( response );
        WUData.FetchUserCategoryLike( ID_of_someone_else, "em", FetchAllUserCategories, GID, PrintError );
    }

    void FetchAllUserCategories( CML response )
    {
        PrintResponse( response );
        WUData.FetchUserGameInfo( ID_of_someone_else, FetchUserUncatgorised, GID, PrintError );
    }

    void FetchUserUncatgorised( CML response )
    {
        PrintResponse( response );
        WUData.FetchUserGlobalInfo( ID_of_someone_else, RemoveAUserField, PrintError );
    }

    void RemoveAUserField( CML response )
    {
        PrintResponse( response );
        WUData.RemoveUserField( ID_of_someone_else, "Marco", "demo", RemoveUserCategories, GID, PrintError );
    }

    void RemoveUserCategories( CML response )
    {
        print( "Removed a field" );
        WUData.RemoveUserCategory( ID_of_someone_else, "demo", gid:GID );
        WUData.RemoveUserCategory( ID_of_someone_else, "", WeAreDone, GID, PrintError );
    }

    void WeAreDone(CML response)
    {
        print( "All Done! If we got this far then good on us! :)" );
    }

    void PrintResponse( CML response ) => print( response.ToString() );
    void PrintError( CMLData response ) => Debug.LogWarning( "Error: " + response.ToString() );
	
}
