using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MBS
{
    public enum WUSActions { RegisterSerial, ValidateRegistration, FetchSerialNumber }

    /// <summary>
    /// This class has been purposely left as empty as possible. There are a number of functions on the server that can return results to Unity
    /// but it makes more sense to let the registrations take place on the server and to keep the game out of the process as much as possible.
    /// </summary>
    static public class WUSerials
    {
        #region PRIVATE CODE
        const string filepath = "wuss_serials/unity_functions.php";
        const string SERIALSConstant = "SERIALS";
        #endregion

        static public void ValidateRegistration( Action<CML> onSuccess, Action<CMLData> onFail = null ) => WPServer.ContactServer( WUSActions.ValidateRegistration, filepath, SERIALSConstant, null, onSuccess, onFail );
        static public void FetchSerialNumber( Action<CML> onSuccess, Action<CMLData> onFail = null ) => WPServer.ContactServer( WUSActions.FetchSerialNumber, filepath, SERIALSConstant, null, onSuccess, onFail );
        static public void RegisterSerial( string serial, Action<CML> onSuccess, Action<CMLData> onFail = null )
        {
            CMLData fields = new CMLData();
            serial = serial.Trim();
            if ( string.IsNullOrEmpty( serial ) )
            {
                fields.Set( "message", "A serial number is required" );
                onFail?.Invoke(fields);
                return;
            }

            fields.Set( "serial", serial );
            WPServer.ContactServer( WUSActions.RegisterSerial, filepath, SERIALSConstant, fields, onSuccess, onFail );
        }
    }
}
