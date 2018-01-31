<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Check if authentification is correct and validate it
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


if($connection_granted == 1)	// If the connection is granted
{
	// SUCCESS
	$_SESSION['connected'] = true;
	$_SESSION['username'] = $username;
	$_SESSION['role'] = $account['role'];
	$_SESSION['user_id'] = getUsernameID($username);
	$_SESSION['user_ip'] = $_SERVER['REMOTE_ADDR'];
	
	$registrationDate = new DateTime($account['creation_date'], new DateTimeZone('Europe/Paris'));
	
	// Get gaming of the player
	$gaming = getGaming($account['current_game'], $account['id']);
	
	$serverDatas = array(
		"Connection granted.",
		$SID,
		$account['role'],
		$account['mail'],
		$registrationDate->format('Y-m-d H:i:s'),
		$previousConnectionDate->format('Y-m-d H:i:s'),
		$gaming['minutes_played']
	);
	
	sendArrayAndFinish($serverDatas);
}

end_script("ERROR: authenticatie mislukt.");


?>