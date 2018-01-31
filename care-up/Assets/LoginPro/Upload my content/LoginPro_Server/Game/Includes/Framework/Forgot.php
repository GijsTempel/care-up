<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Verify if the email address is linked to an account, if so send an email with a link to reinitialize password account.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//------------------------------: GET USER INFORMATION :---------------------------------------------
$mail = $datas[0];
$IP = $_SERVER['REMOTE_ADDR'];

//-------------------------------: SEND LINK TO USER :-----------------------------------------------
$reinitPassword_completed = reinitPassword($mail, $IP);

//-----------------------------: SUCCESS/ERROR MESSAGE :--------------------------------------------
if($reinitPassword_completed)
{
	// SUCCESS
	sendAndFinish("Een link om uw wachtwoord te herstellen is verzonden naar jou e-mailadres.");
}

end_script("Er is iets verkeerd gegaan. Neem contact op met de Care Up support team.");

?>