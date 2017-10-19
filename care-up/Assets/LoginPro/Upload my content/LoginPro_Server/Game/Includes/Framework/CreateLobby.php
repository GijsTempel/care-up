<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script create a server and connects the creator to it
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


$serverName = $datas[0];
$serverCapacity = $datas[1];

// Shutdown the server you could be the creator before
$query = "DELETE FROM ".$_SESSION['Server']." WHERE creator_id = :creator_id";
$parameters = array(':creator_id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// Create a new lobby
$query = "INSERT INTO ".$_SESSION['Server']." (id,creator_id,name,capacity) VALUES (NULL,:creator_id,:name,:capacity)";
$parameters = array('creator_id' => USER_ID, ':name' => $serverName, ':capacity' => $serverCapacity);
$stmt = ExecuteQuery($query, $parameters);

// Get the newly created server id
$query = "SELECT id FROM ".$_SESSION['Server']." WHERE creator_id = :creator_id";
$parameters = array(':creator_id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);
$createdServer = $stmt->fetch();
$createdServerId = $createdServer['id'];

// Join it
$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = :joined_server_id, score = 0 WHERE id = :id";
$parameters = array(':joined_server_id' => $createdServerId, ':id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
$datasToSend = array();
$datasToSend[] = 'Server created!';
$datasToSend[] = $createdServerId;
sendArrayAndFinish($datasToSend);

?>