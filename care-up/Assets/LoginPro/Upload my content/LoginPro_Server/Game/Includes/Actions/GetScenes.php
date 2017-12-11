<?php
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	This script is an example of how to use, get and/or send information to the game
//	It's very simple :
//	Add the name of your action in the "Server.php" script in the ******** ACTIONS ZONE ************
//	Create your script starting from this example and you can do whatever you want
//
//	NOTE : Remember to upload the PHP scripts you change, so it can be executed on your server
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Use those variables (they are defined in the CheckAuthentification.php script) -> (WITHOUT $ !! DO NOT USE IT LIKE THAT: $USERNAME it won't work, use USERNAME)
// USERNAME
// USER_ID
// GAME_ID
// USER_IP

// Use those too (with the $ this time)
// $account (All information about the account table of the current player : $account['id'] OR $account['role'] OR $account['current_game'] OR ...)
// $game (All information about the currently connected game, $game['id'] OR $game['name'] OR $game['version'])



// LET'S BEGIN :
// Notice that $_SESSION['GamingTable'] is set in the script 'ServerSettings.php', if you want to add other tables: add them in 'Server.php' in the ******* TABLES ZONE ********

$query = "SELECT * FROM ".$_SESSION['GameScenes']."";
$stmt = ExecuteQuery($query);

$data = array();

while ($info = $stmt->fetch())
{
	$data[] = $info['SceneID'];
}

sendArrayAndFinish($data);

// That's all ! :)

?>