<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all server contained in the server table to be displayed
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// The player id to be kicked
$playerIdToKick = $datas[0];

// The server id
$myCurrentLobbyId = $account['joined_server_id'];

// Check if the specified server id exists
$query = "SELECT id FROM ".$_SESSION['Server']." WHERE id = :id";
$parameters = array(':id' => $myCurrentLobbyId);
$stmt = ExecuteQuery($query, $parameters);
$serverToJoin = $stmt->fetch();
if(!isset($serverToJoin['id']))
{
	// Save the fact that the server does not exist anymore
	$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = NULL WHERE id = :id";
	$parameters = array(':id' => USER_ID);
	$stmt = ExecuteQuery($query, $parameters);
	
	// Make the player leave the lobby
	end_script("This server is shutdown.");
}

// Check if I'm the server's host
$query = "SELECT creator_id FROM ".$_SESSION['Server']." WHERE id = :id";
$parameters = array(':id' => $myCurrentLobbyId);
$stmt = ExecuteQuery($query, $parameters);
$serverToJoin = $stmt->fetch();
if($serverToJoin['creator_id'] != USER_ID)
{
	// Error if not host
	end_script("You can't kick players if you are not the host.");
}

// Kick the player out
$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = NULL WHERE id = :id";
$parameters = array(':id' => $playerIdToKick);
$stmt = ExecuteQuery($query, $parameters);

// Send the new lobby players list
include_once 'Includes/Framework/GetLobbyPlayers.php';

?>