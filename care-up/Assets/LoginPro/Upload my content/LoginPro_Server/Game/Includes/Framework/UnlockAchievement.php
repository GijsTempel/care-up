<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Unlock achievement with a percentage > 0 (or lock it if percent == 0)
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//---------------------------: GET ACHIEVEMENT INFORMATION :---------------------------------------
$achievementName = $datas[0];
$percent = intval($datas[1], 10);

//------------------------------: SAVE ACHIEVEMENT :-----------------------------------------------
unlockAchievement(USER_ID, $achievementName, $percent);

// Return all the achievement currently unlocked (or with a percent > 0)
include_once 'Includes/Framework/Achievements.php';

?>