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

// Shutdown the server you could be the creator before
$query = "DELETE FROM ".$_SESSION['Server']." WHERE creator_id = :creator_id";
$parameters = array(':creator_id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// Leave the lobby
$query = "UPDATE ".$_SESSION['AccountTable']." SET joined_server_id = NULL WHERE id = :id";
$parameters = array(':id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

SendAndFinish('Server leaved.');

?>