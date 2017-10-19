<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all server contained in the server table to be displayed
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// The server id to join
$serverIdToJoin = $datas[0];

// Check if the server id is specified
if(!isset($serverIdToJoin))
	end_script("JoinLobby: No server id specified.");

// Check if the specified server id exists
$query = "SELECT ".$_SESSION['Server'].".id AS id, ".$_SESSION['AccountTable'].".username AS username, ".$_SESSION['Server'].".capacity AS capacity FROM ".$_SESSION['Server']." JOIN ".$_SESSION['AccountTable']." ON ".$_SESSION['Server'].".creator_id = ".$_SESSION['AccountTable'].".id WHERE ".$_SESSION['Server'].".id = :id";
$parameters = array(':id' => $serverIdToJoin);
$stmt = ExecuteQuery($query, $parameters);
$serverToJoin = $stmt->fetch();
if(!isset($serverToJoin['id']))
	end_script("This server is shutdown.");

// If my current server is not the one I'm trying to connect : check the max players
if($account['joined_server_id'] != $serverIdToJoin)
{
	// Check if the server capacity is reached
	$query = "SELECT COUNT(id) AS players_count FROM ".$_SESSION['AccountTable']." WHERE joined_server_id = :joined_server_id";
	$parameters = array(':joined_server_id' => $serverIdToJoin);
	$stmt = ExecuteQuery($query, $parameters);
	$numberOfPlayersOnThisServer = $stmt->fetch();
	if($serverToJoin['capacity'] <= $numberOfPlayersOnThisServer['players_count'])
		end_script("Maximum players reached : ".$numberOfPlayersOnThisServer['players_count']."/".$serverToJoin['capacity']);
}

// Join the server
$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = :joined_server_id, score = 0 WHERE id = :id";
$parameters = array(':joined_server_id' => $serverIdToJoin, ':id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
$datasToSend = array();
$datasToSend[] = 'Connected!';
$datasToSend[] = $serverToJoin['id'];
$datasToSend[] = $serverToJoin['username'];
sendArrayAndFinish($datasToSend);

?>