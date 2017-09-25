This package 'Serial number license' is designed to protect products by binding the product to hardwareID using serial numbers (DSA signature)


The package is included in itself
- An example of embedding verification of serial numbers (licenses) in the code of the application (game)
- Interface for generating serial numbers (licenses)

Functionality:
- Generation of serial numbers based on the DSA algorithm
- Generation of serial numbers based on a simple algorithm of an asymmetric signature
- Binding a serial number to an apartment identifier
- Support for the expiration of the serial number



=============================================================
- An example of embedding verification of serial numbers (licenses) in the code of the application (game)

\Assets\DevXUnity\Test\Scripts\SceneScript.cs
The SceneScript class is an example of embedding a serial number check in the game code


\Assets\DevXUnity\SerialNumberLicense\SerialNumberVerify.cs
\Assets\DevXUnity\SerialNumberLicense\SerialNumberVerifyDSA.cs
\Assets\DevXUnity\SerialNumberLicense\SerialNumberVerifySimple.cs
Classes SerialNumberVerify, SerialNumberVerifyDSA, SerialNumberVerifySimple is an auxiliary class for validating the serial numbers

Basic methods
		internal bool VerifySignature(string value, string signature)
        internal bool VerifySignature(byte[] value, string signature)
	  

\Assets\DevXUnity\SerialNumberLicense\SerialNumberValidateTools.cs
The SerialNumberValidateTools class implements the verification of serial numbers (licenses) based on hardware_id and the current date

Basic methods
        internal static bool Verify() - Verify current user serial number
        internal static string SerialNumberKey - Set or get - User Serial Number key
        static internal string HardwareID - Device Hardware ID
=============================================================



=============================================================
- Interface for generating serial numbers (licenses)

\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberSigner.cs
\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberSignerDSA.cs
\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberSignerSimple.cs

Classes SerialNumberSigner, SerialNumberSignerDSA, SerialNumberSignerSimple implements the functions of generating serial numbers

Basic methods
        internal void GenerateKeys() - Generate open and close keys
        internal string Sign(string value) - Make serial number

	
\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberGeneratorTools.cs
Class SerialNumberGeneratorTools implements high-level functions for working with licenses

Basic methods
        internal static void UpdateKeys(bool re_create=false, bool as_dsa=false) - Update/Create Keys
        internal static string MakeLicense(string hardware_id, DateTime? expiration_date, string comment, string email) - MakeLicense and save into folder
        internal static List<LicenseInfo> GetLicenseList() - Return all generated licenses
        internal static string CloseKey - Get or set Close key
        internal static string OpenKey - Get or set Open key


\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberGeneratorUI.cs

Class SerialNumberGeneratorUI implements an example of a user interface for working with licenses: "Window / DevXUnityTools-SerialNumbers"

Basic methods
        internal static void LicenseGeneratorShow() - Show License Generator Tab page
        void OnGUI() -  Main GUI
=============================================================
        