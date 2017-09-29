<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all server contained in the server table to be displayed
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// The server id to join
$myCurrentLobbyId = $account['joined_server_id'];

if(!isset($myCurrentLobbyId))
	end_script("GetLobbyPlayers: You must connect to a server first.");

// Shutdown all servers where creator is disconnected
$query = "DELETE FROM ".$_SESSION['Server']." WHERE creator_id IN (SELECT id FROM ".$_SESSION['AccountTable']." WHERE last_activity < DATE_SUB(NOW(), INTERVAL 2 MINUTE))";
$parameters = array();
$stmt = ExecuteQuery($query, $parameters);

// Kick all disconnected players from all lobbies
$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = NULL WHERE last_activity < DATE_SUB(NOW(), INTERVAL 2 MINUTE)";
$parameters = array();
$stmt = ExecuteQuery($query, $parameters);

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

// Check if I am the server's host
$query = "SELECT creator_id FROM ".$_SESSION['Server']." WHERE id = :id";
$parameters = array(':id' => $myCurrentLobbyId);
$stmt = ExecuteQuery($query, $parameters);
$serverToJoin = $stmt->fetch();
$serverHost = getUsernameFromId($serverToJoin['creator_id']);

// Get all players of the lobby
$query = "SELECT id, username, score FROM ".$_SESSION['AccountTable']." WHERE joined_server_id = :joined_server_id ORDER BY score";
$parameters = array(':joined_server_id' => $myCurrentLobbyId);
$stmt = ExecuteQuery($query, $parameters);

$datasToSend = array();
$datasToSend[] = $serverHost;					// The username of the server host
foreach($stmt as $player)
{
	$datasToSend[] = $player["id"];				// The id of the player
	$datasToSend[] = $player["username"];		// The name of the player
	$datasToSend[] = $player["score"];			// The score of the player
}

sendArrayAndFinish($datasToSend);

?>