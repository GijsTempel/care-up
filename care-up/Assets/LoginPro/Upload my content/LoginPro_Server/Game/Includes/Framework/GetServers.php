<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all server contained in the server table to be displayed
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Shutdown all servers where creator is disconnected
$query = "DELETE FROM ".$_SESSION['Server']." WHERE creator_id IN (SELECT id FROM ".$_SESSION['AccountTable']." WHERE last_activity < DATE_SUB(NOW(), INTERVAL 2 MINUTE))";
$parameters = array();
$stmt = ExecuteQuery($query, $parameters);

// Kick all disconnected players from all lobbies
$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = NULL WHERE last_activity < DATE_SUB(NOW(), INTERVAL 2 MINUTE)";
$parameters = array();
$stmt = ExecuteQuery($query, $parameters);

// Get all servers
$query = "SELECT * FROM ".$_SESSION['Server'];
$parameters = array();
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
$datasToSend = array();
$noServerFound = true;
foreach($stmt as $row)
{
	// If it's the first server of the list : message = reports found
	if($noServerFound)
	{
		$noServerFound = false;
		$datasToSend[] = "Servers list refreshed!";
	}
	
	$datasToSend[] = $row["id"];				// The id of the server
	$datasToSend[] = $row["name"];				// The name of the server
}

// If no server exists : message = no server
if($noServerFound)
{
	$datasToSend[] = "No server available.";
}

sendArrayAndFinish($datasToSend);

?>