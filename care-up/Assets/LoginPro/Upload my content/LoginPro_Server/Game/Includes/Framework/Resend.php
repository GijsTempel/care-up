<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Resend the email account activation
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//------------------------------: GET USER INFORMATION :--------------------------------------------
$username = $datas[0];
$id = getUsernameID($username);

//------------------------------: SEND EMAIL TO USER :----------------------------------------------
$sendingInformation_completed = sendAccountActivationEmail($id);

//-----------------------------: SUCCESS/ERROR MESSAGE :--------------------------------------------
if($sendingInformation_completed)
{
	// SUCCESS
	sendAndFinish("Een link om je account te activeren is verzonden naar je e-mailadres.");
}

end_script("Er ging iets mis. Neem contact op met het Care Up support team.");

?>