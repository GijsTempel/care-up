<?php
$ServerScriptCalled = 1;
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : 	Validate user email by clicking a link in a validation email, then generate a new password and send it to the client
//			This script is accessible from the outside (as well as Script.php, ValidateAccountByURL.php and ValidateIPByURL.php)
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


$ACTION = 'ReceiveNewPassword';
$GenerateSID=1;
$NotInGame=1;
include_once './Server.php';

?>