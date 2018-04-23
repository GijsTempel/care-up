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
$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE id = :account_id";
$parameters = array(':account_id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);
$row = $stmt->fetch();

// SUCCESS
if(isset($row['id']))
{
	// Information to get from the server (we use an array because there is multiple datas to get)
	$dataToGetFromServer = array();
	$dataToGetFromServer[] = $row["tutorialCompleted"];
	
	// IMPORTANT ! In order to send just a message, use the function sendAndFinish("The message you want to send") (See the PHP script just besides called "SendData.php")
	// IMPORTANT ! If you want to send MULTIPLE DATAS from the server to the game use sendArrayAndFinish, like that :
	sendArrayAndFinish($dataToGetFromServer);
}

// ERROR
end_script("GetData: Unable to get information back.");

// That's all ! :)

?>