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

// Get all messages of the server
$query = "SELECT ".$_SESSION['AccountTable'].".username AS username, ".$_SESSION['ChatMessage'].".date AS date, ".$_SESSION['ChatMessage'].".message AS message FROM ".$_SESSION['ChatMessage']." JOIN ".$_SESSION['AccountTable']." ON ".$_SESSION['ChatMessage'].".account_id = ".$_SESSION['AccountTable'].".id WHERE ".$_SESSION['ChatMessage'].".server_id = :server_id";
$parameters = array(':server_id' => $myCurrentLobbyId);
$stmt = ExecuteQuery($query, $parameters);
$datasToSend = array();
$datasToSend[] = 'List of messages';
foreach($stmt as $message)
{
	$datasToSend[] = $message["date"];			// The date of the message
	$datasToSend[] = $message["username"];		// The username of the player
	$datasToSend[] = $message["message"];		// The message
}

sendArrayAndFinish($datasToSend);

?>