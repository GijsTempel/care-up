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
$serial = $datas[0];

$query = "SELECT * FROM ".$_SESSION['SerialKeys']." WHERE SerialString = :serial_string";
$parameters = array(':serial_string' => $serial);
$stmt = ExecuteQuery($query, $parameters);

$info = $stmt->fetch();

// SUCCESS
if(isset($info['ID']))
{
	$query = "SELECT * FROM ".$_SESSION['AccountToSerials']." WHERE SerialID = :serial_id";
	$parameters = array(':serial_id' => $info['ID']);
	$stmt = ExecuteQuery($query, $parameters);
	$timesUsed = $stmt->rowCount();
	
	if ($timesUsed < $info['AccountNumber'])
	{
		$parameters = array(':account_id' => $account['id'], ':serial_id' => $info['ID']);
		$query = "SELECT * FROM ".$_SESSION['AccountToSerials']." WHERE AccountID = :account_id AND SerialID = :serial_id";
		$stmt = ExecuteQuery($query, $parameters);
		
		if (!$stmt->fetch())
		{
			$query = "INSERT INTO ".$_SESSION['AccountToSerials']." (AccountID, SerialID) VALUES (:account_id, :serial_id)";
			$stmt = ExecuteQuery($query, $parameters);
			sendAndFinish("KeyAccepted");
		}
		else
		{
			sendAndFinish("Duplicate query");
		}
	}
	else
	{
		end_script("Serial key is already being used.");
	}
}

end_script("Wrong serial key.");

?>