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
// The array '$datas' is the same as the one you set in your C# script, use it like that:
$data1 = $datas[0];
$data2 = $datas[1];
$data3 = $datas[2];

// Notice that $_SESSION['GamingTable'] is set in the script 'ServerSettings.php', if you want to add other tables: add them in 'Server.php' in the ******* TABLES ZONE ********
$query = "UPDATE ".$_SESSION['GamingTable']." SET data1 = :data1, data2 = :data2, data3 = :data3 WHERE game_id = :game_id AND account_id = :account_id";
$parameters = array(':data1' => $data1,':data2' => $data2,':data3' => $data3,':game_id' => GAME_ID,':account_id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
// IMPORTANT ! In order to send just a message, use the function sendAndFinish("The message you want to send"), like that :
// IMPORTANT ! If you want to send MULTIPLE DATAS from the server to the game use sendArrayAndFinish (See the PHP script just besides called "GetData.php")
sendAndFinish("Information saved!");

// That's all ! :)

?>