<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all achievements made by the current user
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


$query = "SELECT * FROM ".$_SESSION['AchievementsTable']." WHERE account_id = :account_id";
$parameters = array(':account_id' => USER_ID);
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
$achievements = array();
$noAchievementFound = true;
foreach($stmt as $row)
{
	// If it's the first achievement of the list : message = achievements found
	if($noAchievementFound)
	{
		$noAchievementFound = false;
		$achievements[] = "Achievements received.";
	}
	
	$achievements[] = $row["name"];			// The name of the achievement
	$achievements[] = $row["percent"];		// The percent achieved
	$achievements[] = $row["date"];			// The date of the achievement
}

// If no achievement exists : message = no achievement
if($noAchievementFound)
{
	$achievements[] = "No achievements yet.";
}

sendArrayAndFinish($achievements);

?>