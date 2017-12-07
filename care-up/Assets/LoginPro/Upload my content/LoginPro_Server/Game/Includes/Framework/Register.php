<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

//////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Accept the client as new user, if the mail and login aren't already taken
//
//////////////////////////////////////////////////////////////////////////////////////////


//------------------------------: GET USER INFORMATION :---------------------------------------------
$mail = $datas[0];
$username = $datas[1];
$password = $datas[2];
$IP = $_SERVER['REMOTE_ADDR'];

//--------------------------------: REGISTER USER :--------------------------------------------------
$registration_completed = register($mail, $username, $password, $IP);

//---------------------------: ACCEPTED/REFUSED MESSAGE :--------------------------------------------
if($registration_completed)
{
	// SUCCESS
	sendAndFinish("Registratie gelukt! Er is een link naar je e-mailadres gestuurd om je account te activeren.");
}

end_script("Er ging iets mis. Neem contact op met het Care Up support team.");

?>