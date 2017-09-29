<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Check authentification, if correct : change information
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Get gaming of the player
$gameId = $account['current_game'];
$accountId = $account['id'];
$gaming = getGaming($gameId, $accountId);

// Current date
$now = strtotime(date('Y-m-d H:i:s'));
$previousConnectionDate = strtotime($account['last_connection_date']);
$minutesPlayedDuringLastSession = round(($now - $previousConnectionDate) / 60);

$minutesPlayedBeforeLastSession = intval($gaming['minutes_played_earlier'], 10);
$minutesPlayed = $minutesPlayedBeforeLastSession + $minutesPlayedDuringLastSession;

// Update the time played
$query = "UPDATE ".$_SESSION['GamingTable']." SET minutes_played = :minutes_played WHERE game_id = :game_id AND account_id = :account_id";
$parameters = array(':minutes_played' => $minutesPlayed,':game_id' => $gameId,':account_id' => $accountId);
$stmt = ExecuteQuery($query, $parameters);

// Send number of minutes played to the player
sendAndFinish($minutesPlayed);

?>