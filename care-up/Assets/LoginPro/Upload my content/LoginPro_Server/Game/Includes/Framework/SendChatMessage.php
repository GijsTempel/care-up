<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all server contained in the server table to be displayed
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// The message
$message = $datas[0];

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

// Create a new lobby
$query = "INSERT INTO ".$_SESSION['ChatMessage']." (id,server_id,account_id,message,date) VALUES (NULL,:server_id,:account_id,:message,NOW())";
$parameters = array('server_id' => $serverToJoin['id'], ':account_id' => USER_ID, ':message' => $message);
$stmt = ExecuteQuery($query, $parameters);

// Send the chat
include_once 'Includes/Framework/GetChat.php';

?>