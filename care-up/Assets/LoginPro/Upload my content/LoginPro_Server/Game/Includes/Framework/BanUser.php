<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	This script allow administrators to ban a player by specifying his username
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Verify administrator
if(!isset($_SESSION['role']) || $_SESSION['role'] != 'Admin')
	end_script("BanUser: Only administrators can have access to administration.");

// Username to ban
$username = $datas[0];
if(!isset($username))
	end_script("BanUser: username not set.");

// Check account exists
$id = getUsernameID($username);
if($id == "") { end_script('This username is not linked to any account.'); }

// Set flag "banned" of the account as 1
$query = "UPDATE ".$_SESSION['AccountTable']." SET banned=1 WHERE username = :username";
$parameters = array(':username' => $username);
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
sendAndFinish("User banned.");

?>