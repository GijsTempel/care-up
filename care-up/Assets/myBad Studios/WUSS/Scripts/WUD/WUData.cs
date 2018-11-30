using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace MBS {
    static public class WUData {

        public enum WUDActions {
            Dummy,
            //These functions work on data that is shared by all users. I.e. what enemies to spawn
            FetchSharedField,
            FetchSharedCategory,
            FetchSharedCategoryLike,
            FetchAllSharedInfo,
            RemoveSharedField,
            RemoveSharedCategory,
            SaveSharedData,
            StoreSharedImage,
            DeleteSharedImage,

            //These functions are per player. I.e. high score, gold etc
            FetchField,
            FetchCategory,
            FetchCategoryLike,
            FetchGameInfo,
            FetchGlobalInfo,
            FetchEverything,
            RemoveField,
            RemoveCategory,
            RemoveGameInfo,
            SaveData,
            StoreImage,
            DeleteImage,

            //These functions are for use on OTHER PLAYERS DATA
            FetchUserField,
            FetchUserCategory,
            FetchUserCategoryLike,
            FetchUserGlobalInfo,
            FetchUserGameInfo,
            RemoveUserField,
            RemoveUserCategory,
            SaveUserData,
            StoreUserImage,
            DeleteUserImage,

            //Related to storing and retrieving Images
            FetchImage,

            Count
        }

        public enum EWUDImageTypes :byte { png, jpg }

        const string filepath = "wuss_data/unity_functions.php";
        const string DATAConstant = "DATA";

        static public bool WUDataPro = false;

        #region worker functions

        /// <summary>
        /// This function tells the server if we are in PRO mode or not.
        /// When true it changes the format of the returned data and thus all high level functions will be broken
        /// The changes are cosmetic only and only meant for advanced users who prefer using the low level functions
        /// </summary>
        static CMLData ProUpgrade( CMLData data )
        {
            if ( !WUDataPro )
                return data;
            if ( null == data )
                data = new CMLData();
            data.Seti( "WUDPRO", 1 );
            return data;
        }

        /*
		 * This will fetch a single field from the online database
		 * Required params:
		 * field_name - The name of the field to fetch
		 * cat - The category this stat is under
		 * gid - The game this info belongs to (Optional). 0 for global info
		 */
        static void _fetchField( WUDActions action, string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            CMLData data = new CMLData();
            data.Set( "fid", field_name );
            data.Set( "cat", cat );
            data.Seti( "gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            ProUpgrade( data );
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, data, response, errorResponse );
        }

        /*
		 * This will fetch an entire category of field from the online database
		 * Required params:
		 * cat - The category to fetch
		 * gid - The game this info belongs to (Optional). 0 for global info
		 */
        static void _fetchCategory( WUDActions action, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            CMLData data = new CMLData();
            data.Set( "cat", cat );
            data.Seti( "gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            ProUpgrade( data );
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, data, response, errorResponse );
        }

        /*
		 * This will fetch all data stored in all categories for a specified game
		 * Required params:
		 * gid - The game this info belongs to (Optional). 0 for global info
		 * Defaults to the value set in WUDServer.game_id if less than 0
		 */
        static void _fetchAllInfo( WUDActions action, Action<CML> response, int gid = -1, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            CMLData data = new CMLData();
            data.Seti( "gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            ProUpgrade( data );
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, data, response, errorResponse );
        }

        /*
		 * This will permanently remove the specified field from the database
		 * Required params:
		 * field_name - The name of the field to remove
		 * cat - The category to fetch
		 * gid - The game this info belongs to (Optional). 0 for global info
		 * Defaults to the value set in WUDServer.game_id if less than 0
		 */
        static void _removeField( WUDActions action, string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            CMLData data = new CMLData();
            data.Set( "fid", field_name );
            data.Set( "cat", cat );
            data.Seti( "gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            ProUpgrade( data );
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, data, response, errorResponse );
        }

        /*
		 * This will permanently remove from the database all fields under the
		 * specified category for the given game
		 * Required params:
		 * cat - The category to remove
		 * gid - The game this category belongs to (Optional). 0 for global info
		 * Defaults to the value set in WUDServer.game_id if less than 0
		 */
        static void _removeCategory( WUDActions action, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            CMLData data = new CMLData();
            data.Set( "cat", cat );
            data.Seti( "gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            ProUpgrade( data );
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, data, response, errorResponse );
        }

        /*
		 * This will update one or more existing fields or add it to the database
		 * if it doesn't exist already. fields can contain as many fields as you
		 * like but must go under the same category and belong to the same game.
		 * Required params:
		 * cat - The name of the category to store the fields under
		 * fields - the names and values to store in the database
		 * gid - The game this category belongs to (Optional). 0 for global info
		 * Defaults to the value set in WUDServer.game_id if less than 0
		 */
        static void _updateCategory( WUDActions action, string cat, CMLData fields, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            if ( null == fields )
            {
                StatusMessage.Message = "No fields specified to update";
                return;
            }

            fields.Set( "cat", cat );
            fields.Seti( "gid", gid );
            if ( uid > 0 )
                fields.Seti( "uid", uid );
            ProUpgrade( fields );
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, fields, response, errorResponse );
        }

        /*
        * This will fetch all data stored in all categories for game 0
        * The intention is that you use game_id 0 for global settings
        * and values larger than 0 for your actual game data
        */
        static void _fetchGlobalInfo( WUDActions action, Action<CML> response, Action<CMLData> errorResponse = null , int uid = -1)
        {
            CMLData data = (uid > 0) ? new CMLData() : null;
            if ( uid > 0 )
                data.Seti( "uid", uid );           
            WPServer.ContactServer( action.ToString(), filepath, DATAConstant, ProUpgrade( data ), response, errorResponse );
        }

        static void _storeImage( WUDActions action, Texture2D image, string fieldname, string cat = "", EWUDImageTypes extension = EWUDImageTypes.png, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null, int quality = 75, int uid = -1 )
        {
            if ( null == image )
            {
                CMLData noimage = new CMLData();
                noimage.data_type = "DATA";
                noimage.Set( "message", "Image cannot be null" );
                errorResponse?.Invoke( noimage );
                return;
            }

            byte [] bytes = extension == EWUDImageTypes.png ? image.EncodeToPNG() : image.EncodeToJPG( quality );
            string encoded = Convert.ToBase64String( bytes );

            CMLData data = new CMLData();
            data.Set( "imgfd", fieldname );
            data.Set( "imgnm", image.name );
            data.Set( "imgex", extension.ToString() );
            data.Set( "imgdt", encoded );
            data.Set( "cat", cat );
            data.Seti("gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            WPServer.ContactServer( action, filepath, DATAConstant, data, response, errorResponse );
        }

        static void _deleteImage(WUDActions action, string field, string cat = "", int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null, int uid = -1 )
        {
            CMLData data = new CMLData();
            data.Set( "key", field );
            data.Set( "cat", cat );
            data.Seti("gid", gid );
            if ( uid > 0 )
                data.Seti( "uid", uid );
            WPServer.ContactServer( action, filepath, DATAConstant, data, response, errorResponse );
        }

        #endregion

        #region shared data
        static public void FetchSharedField(string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null) =>
            _fetchField(WUDActions.FetchSharedField, field_name, cat, response, gid, errorResponse);

        static public void FetchSharedCategory( string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchCategory( WUDActions.FetchSharedCategory, cat, response, gid, errorResponse );

        static public void FetchSharedCategoryLike( string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchCategory( WUDActions.FetchSharedCategoryLike, cat, response, gid, errorResponse );

        static public void FetchAllSharedInfo( Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchAllInfo(WUDActions.FetchAllSharedInfo, response, gid, errorResponse);

        static public void RemoveSharedField( string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _removeField(WUDActions.RemoveSharedField, field_name, cat, response, gid, errorResponse);

        static public void RemoveSharedCategory( string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _removeCategory(WUDActions.RemoveSharedCategory, cat, response, gid, errorResponse);

        static public void UpdateSharedCategory( string cat, CMLData fields, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null ) =>
            _updateCategory( WUDActions.SaveSharedData, cat, fields, gid, response, errorResponse );

        static public void StoreSharedImage( Texture2D image, string field_name, string cat, EWUDImageTypes extension, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null, int quality= 75 ) =>
            _storeImage( WUDActions.StoreSharedImage, image, field_name, cat, extension, gid, response, errorResponse, quality );

        static public void DeleteSharedImage( string field_name, string cat, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null ) =>
            _deleteImage( WUDActions.DeleteSharedImage, field_name, cat, gid, response, errorResponse );
        #endregion

        #region personal data
        static public void FetchField( string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchField( WUDActions.FetchField, field_name, cat, response, gid, errorResponse );

        static public void FetchCategory( string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchCategory( WUDActions.FetchCategory, cat, response, gid, errorResponse );

        static public void FetchCategoryLike( string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchCategory( WUDActions.FetchCategoryLike, cat, response, gid, errorResponse );

        static public void FetchGameInfo( Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchAllInfo( WUDActions.FetchGameInfo, response, gid, errorResponse );
		
		static public void FetchGlobalInfo( Action<CML> response, Action<CMLData> errorResponse = null) =>
			_fetchGlobalInfo( WUDActions.FetchGlobalInfo, response, errorResponse);

        static public void RemoveField( string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _removeField( WUDActions.RemoveField, field_name, cat, response, gid, errorResponse );

        static public void RemoveCategory( string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _removeCategory( WUDActions.RemoveCategory, cat, response, gid, errorResponse );

        static public void UpdateCategory( string cat, CMLData fields, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null ) =>
            _updateCategory( WUDActions.SaveData, cat, fields, gid, response, errorResponse );

        static public void StoreImage( Texture2D image, string field_name, string cat, EWUDImageTypes extension, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null, int quality = 75 ) =>
            _storeImage( WUDActions.StoreImage, image, field_name, cat, extension, gid, response, errorResponse, quality );

        static public void DeleteImage( string field_name, string cat, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null ) =>
            _deleteImage( WUDActions.DeleteImage, field_name, cat, gid, response, errorResponse );

        /*
		 * Use this when you want to generate stats about the user's game play
		 * behavior. This function will return absolutely everything you have ever
		 * stored for this user. All global settings and all data in all categories
		 * for all your games the player has ever played
		 */
        static public void FetchEverything(Action<CML> response, Action<CMLData> errorResponse = null) =>
			WPServer.ContactServer(WUDActions.FetchEverything.ToString(), filepath, DATAConstant, ProUpgrade( null ), response, errorResponse);

        /*
		 * This will permanently remove from the database all info for the selected game
		 * Required params:
		 * gid - The game this category belongs to (Optional). 0 for global info
		 * Defaults to the value set in WUDServer.game_id if less than 0
		 */
        static public void RemoveGameData(int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null)
		{
			CMLData data = new CMLData();
			data.Seti  ("gid"  , gid);
            ProUpgrade( data );
            WPServer.ContactServer(WUDActions.RemoveGameInfo, filepath, DATAConstant, data, response, errorResponse);
		}
                
        #endregion

        #region authoritative host access functions

        static public void FetchUserField( int uid, string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
         _fetchField( WUDActions.FetchUserField, field_name, cat, response, gid, errorResponse, uid );

        static public void FetchUserCategory( int uid, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchCategory( WUDActions.FetchUserCategory, cat, response, gid, errorResponse, uid );

        static public void FetchUserCategoryLike( int uid, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchCategory( WUDActions.FetchUserCategoryLike, cat, response, gid, errorResponse, uid );

        static public void FetchUserGameInfo( int uid, Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _fetchAllInfo( WUDActions.FetchUserGameInfo, response, gid, errorResponse, uid );

        static public void FetchUserGlobalInfo( int uid, Action<CML> response, Action<CMLData> errorResponse = null ) =>
            _fetchGlobalInfo( WUDActions.FetchUserGlobalInfo, response, errorResponse, uid );

        static public void RemoveUserField( int uid, string field_name, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _removeField( WUDActions.RemoveUserField, field_name, cat, response, gid, errorResponse, uid );

        static public void RemoveUserCategory( int uid, string cat = "", Action<CML> response = null, int gid = -1, Action<CMLData> errorResponse = null ) =>
            _removeCategory( WUDActions.RemoveUserCategory, cat, response, gid, errorResponse, uid );

        static public void UpdateUserCategory( int uid, string cat, CMLData fields, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null ) =>
            _updateCategory( WUDActions.SaveUserData, cat, fields, gid, response, errorResponse, uid );

        static public void StoreUserImage( int uid, Texture2D image, string field_name, string cat, EWUDImageTypes extension, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null, int quality = 75 ) =>
            _storeImage( WUDActions.StoreUserImage, image, field_name, cat, extension, gid, response, errorResponse, quality, uid );

        static public void DeleteUserImage(int uid, string field_name, string cat, int gid = -1, Action<CML> response = null, Action<CMLData> errorResponse = null ) =>
            _deleteImage( WUDActions.DeleteUserImage, field_name, cat, gid, response, errorResponse, uid );

        #endregion

        #region HIGH LEVEL API
        /// The following functions are placed here to help people who are not familiar with CML to hit the ground running
        /// These functions are bloated and absolute overkill and their only purpose is to give new people functions with names
        /// that are clear and easy to identify. 
        /// 
        /// I recommend finding the CML and accompanying Examples folders and learning CML ASAP. 
        /// 
        /// The following are fine for starting out with and you can always continue to use them as is... but if you decide to
        /// learn CML (which I use in ALL my kits and is definitely worth learning), this asset will shoot to a whole new level of awesome!

        static public List<CMLData> ExtractAllGames( CML raw_data ) => raw_data.AllNodesOfType( "_GAME_" );
        static public CMLData ExtractFirstGame( CML raw_data ) => raw_data.GetFirstNodeOfType( "_GAME_" );

        // Basic field getters...
        static public int GetFieldi( List<CMLData> categories, string field_name, string cat = "" ) => (int)GetFieldf( categories, field_name, cat );
        static public int GetFieldi( CML data, string field_name, string cat = "", int gid = 0 ) => (int)GetFieldf( data, field_name, cat, gid );
        static public int GetFieldi( CML data, string field_name, string cat = "", string game_name = "" ) => (int)GetFieldf( data, field_name, cat, game_name );

        // This will search through the game's global settings for the field you specify 
        static public CMLData ExtractAGameByField( CML raw_data, string field, string value )
        {
            List<CMLData> games = raw_data.AllNodesOfType( "_GAME_" );
            if ( null == games )
                return null;

            foreach ( CMLData game in games )
            {
                List<CMLData> categories = ExtractCategoriesForGame( raw_data, game.ID );
                if ( null != categories )
                {
                    foreach ( CMLData category in categories )
                    {
                        if ( category.String( field ) == value && game.String( "category" ) == string.Empty )
                            return game;
                    }
                }
            }
            return null;
        }

        /*
		 * This will search through the game's global settings for a field named "name"
		 * and will return it if it's name matches the value you specify
		 * All sub categories are ignored
		 */
        static public CMLData ExtractAGameByName( CML raw_data, string name ) => ExtractAGameByField( raw_data, "name", name );
        static public List<CMLData> ExtractAllCategories( CML raw_data ) => raw_data.AllNodesOfType( "_CATEGORY_" );

        /*
		 * First find the game node then pass it's ID as the second parameter
		 * Example:
		 *	List<CMLData> categories;
		 *	CMLData myGame = WUData.ExtractAGameByName(raw_data, "PacMan");
		 *	if (null != myGame) categories = WUData.ExtractCategoriesForGame(raw_data, myGame.ID);
		 */
        static public List<CMLData> ExtractCategoriesForGame( CML raw_data, int ID ) => raw_data.Children( ID );

        // For this function to work, the game must have a field called "name" in the game's global settings		 
        static public List<CMLData> ExtractCategoriesByGameName( CML raw_data, string name )
        {
            CMLData myGame = ExtractAGameByName( raw_data, name );
            if ( null == myGame )
                return null;

            return raw_data.Children( myGame.ID );
        }

        static public CMLData ExtractCategoryFromGame( CML raw_data, string category_name = "", int id = 1 )
        {
            List<CMLData> categories = raw_data.Children( id );
            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == category_name )
                    return category;

            return null;
        }

        static public CMLData ExtractCategoryFromGameName( CML raw_data, string category_name = "", string game_name = "Global" )
        {
            List<CMLData> categories = ExtractCategoriesByGameName( raw_data, game_name );
            if ( null == categories )
                return null;

            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == category_name )
                    return category;

            return null;
        }

        static public string GetField( List<CMLData> categories, string field_name, string cat = "" )
        {
            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == cat )
                    return category.String( field_name );
            return string.Empty;
        }

        static public float GetFieldf( List<CMLData> categories, string field_name, string cat = "" )
        {
            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == cat )
                    return category.Float( field_name );
            return 0;
        }

        static public string GetField( CML data, string field_name, string cat = "", int gid = 0 )
        {
            List<CMLData> categories = ExtractCategoriesForGame( data, gid );
            if ( null == categories )
                return "";

            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == cat )
                    return category.String( field_name );
            return string.Empty;
        }

        static public float GetFieldf( CML data, string field_name, string cat = "", int gid = 0 )
        {
            List<CMLData> categories = ExtractCategoriesForGame( data, gid );
            if ( null == categories )
                return 0;

            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == cat )
                    return category.Float( field_name );
            return 0;
        }

        static public string GetField( CML data, string field_name, string cat = "", string game_name = "" )
        {
            List<CMLData> categories = ExtractCategoriesByGameName( data, game_name );
            if ( null == categories )
                return "";

            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == cat )
                    return category.String( field_name );
            return string.Empty;
        }

        static public float GetFieldf( CML data, string field_name, string cat = "", string game_name = "" )
        {
            List<CMLData> categories = ExtractCategoriesByGameName( data, game_name );
            if ( null == categories )
                return 0;

            foreach ( CMLData category in categories )
                if ( category.String( "category" ) == cat )
                    return category.Float( field_name );
            return 0;
        }

        #endregion
    }

}