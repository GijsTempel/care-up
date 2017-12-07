<?php
$ServerScriptCalled = 1;
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : 	Validate user account by clicking a link in a validation email
//			This script is accessible from the outside (as well as Script.php and ValidateIPByURL.php)
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//--------------------------------------: INCLUDE FILES :-------------------------------------------------
$GenerateSID = 1;
include_once 'Includes/Functions.php'; 				// Include protection functions and tools functions
include_once 'Includes/Session.php'; 				// Begin client session (or return to index if not session ID received)
include_once 'Includes/ServerSettings.php'; 	// Include server settings to get the configuration in order to connect to the database
include_once 'Includes/InitServerConnection.php'; 	// Initialize the connection with the database


//-----------------------------------: CHECK DATA PRESENCE :----------------------------------------------
if(!isset($_GET['username'])) { end_script('Username not received.'); }
if(!isset($_GET['code'])) { end_script('Code not received.'); }

//--------------------------------: PROTECT AGAINST INJECTION :-------------------------------------------
$username = $_GET['username'];
$code = $_GET['code'];

//------------------------------------: GET USER ACCOUNT :------------------------------------------------
$account = getAccount($username);
if(is_null($account)) { end_script('Login mislukt. De gebruikersnaam is niet gelinkt aan een account.'); }
if($account['validated']!=0) { end_script('Je account is al geactiveerd. Je kunt inloggen met je account en wachtwoord.'); }
if(strcmp($account['validation_code'],$code)) { end_script('Je code is niet correct. Neem contact op met het het support team.'); }
if(checkMailExists($account['mail'])) { end_script('Registratie mislukt. Dit e-mailadres is al in gebruik.'); }

//------------------------------------: CHECK IP BLOCKED :------------------------------------------------
$id = $account['id'];
$IP = $_SERVER['REMOTE_ADDR'];
checkAccountValidationAttempts($id, $IP);
increaseAttempts($id, $IP, 'Validation');

//------------------------------------: ACTIVATE ACCOUNT :------------------------------------------------
$activation_completed = activateAccount($account['id'], $IP, $account['mail']);
if($activation_completed)
{
	echo("Je account is succesvol geactiveerd. Je kunt nu inloggen.");
	// Close connection to database properly
	$_SESSION['databaseConnection'] = null;
	// Ensure the end of the current script
	die();
	exit(0);
}
else { end_script('Account registratie mislukt.'); }

?>