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
	end_script("ChangeUserRole: Only administrators can have access to administration.");

// Datas
$username = $datas[0];
$role = $datas[1];

// Check datas
if(!isset($username))
	end_script("ChangeUserRole: username not set.");
if(!isset($role))
	end_script("ChangeUserRole: role not set.");

// Check account exists
$id = getUsernameID($username);
if($id == "") { end_script('This username is not linked to any account.'); }

// Set the role
$query = "UPDATE ".$_SESSION['AccountTable']." SET role=:role WHERE username = :username";
$parameters = array(':role' => $role,':username' => $username);
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
sendAndFinish("User role set to '".$role."'.");

?>