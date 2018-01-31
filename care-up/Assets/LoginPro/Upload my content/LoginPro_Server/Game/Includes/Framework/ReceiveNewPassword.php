<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Verify if the email address is linked to an account, if so send an email with a link to reinitialize password account.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//----------------------------: CHECK RECEIVED INFORMATION :-----------------------------------------
// Verify encrypted data presence
if(!isset($_GET['mail'])) { end_script('Wachtwoord herstellen mislukt. Er is geen e-mailadres opgegeven.'); }
if(!isset($_GET['code'])) { end_script('Wachtwoord herstellen mislukt. Er is geen code opgegeven.'); }


//------------------------------: GET USER INFORMATION :---------------------------------------------
$mail = $_GET['mail'];
$code = $_GET['code'];
$IP = $_SERVER['REMOTE_ADDR'];


//-------------------------------: SEND LINK TO USER :-----------------------------------------------
$sendPassword_completed = sendPassword($mail, $code, $IP);
if($sendPassword_completed)
{
	echo("Wachtwoord herstellen is gelukt! Er is een nieuw wachtwoord naar je e-mailadres gestuurd.");
	// Close connection to database properly
	$_SESSION['databaseConnection'] = null;
	// Ensure the end of the current script
	die();
	exit(0);
}
else { end_script('Er ging iets mis. Neem contact op met het Care Up support team'); }

?>