<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all reports contained in the report table to be treated by administrators
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$query = "SELECT ".$_SESSION['Friends'].".id_asker AS friend_id, ".$_SESSION['AccountTable'].".username AS friend_name, ".$_SESSION['AccountTable'].".last_activity AS last_activity, ".$_SESSION['Friends'].".status AS status, 'False' AS my_demand FROM ".$_SESSION['Friends']." JOIN ".$_SESSION['AccountTable']." ON id_asker = ".$_SESSION['AccountTable'].".id WHERE ".$_SESSION['Friends'].".id_asked = :user_id_asked UNION SELECT ".$_SESSION['Friends'].".id_asked AS friend_id, ".$_SESSION['AccountTable'].".username AS friend_name, ".$_SESSION['AccountTable'].".last_activity AS last_activity, ".$_SESSION['Friends'].".status AS status, 'True' AS my_demand FROM ".$_SESSION['Friends']." JOIN ".$_SESSION['AccountTable']." ON id_asked = ".$_SESSION['AccountTable'].".id WHERE ".$_SESSION['Friends'].".id_asker = :user_id_asker";
$parameters = array(':user_id_asked' => USER_ID,':user_id_asker' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// Current date
$now = strtotime(date('Y-m-d H:i:s'));

// SUCCESS
$datasToSend = array();
$noFriendFound = true;
foreach($stmt as $row)
{
	// If it's the first achievement of the list : message = reports found
	if($noFriendFound)
	{
		$noFriendFound = false;
		$datasToSend[] = "Friend list received.";
	}
	
	// Is my friend connected ?
	$lastActivityDate = strtotime($row['last_activity']);
	$is_connected = round(($now - $lastActivityDate) / 60) < 2 ? "True" : "False";
	
	$datasToSend[] = $row["friend_id"];					// The id of the report (NOT the reporter, the report)
	$datasToSend[] = $row["friend_name"];				// The date of the report
	$datasToSend[] = $is_connected;						// Is my friend connected
	$datasToSend[] = $row["status"];					// The reporter username
	$datasToSend[] = $row["my_demand"];					// If it is me who sent the friend request
}

// If no achievement exists : message = no achievement
if($noFriendFound)
{
	$datasToSend[] = "You have no friend currently.";
}

sendArrayAndFinish($datasToSend);

?>