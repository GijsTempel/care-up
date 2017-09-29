<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Check authentification, if correct : accept/delete friend request
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//------------------------------: GET REQUEST INFORMATION :---------------------------------------------
$friend_id = $datas[0];
$accepted = $datas[1];

//---------------------------------: VERIFICATION :--------------------------------------------------
if(!is_numeric($friend_id)) { end_script('The id of your friend must be numeric.'); }
if($accepted != 'True' && $accepted != 'False') { end_script('Response must be True or False.'); }

//------------------------------: UPDATE REQUEST :-----------------------------------------------
if($accepted == 'True')
{
	$query = "UPDATE ".$_SESSION['Friends']." SET status = :status WHERE id_asker = :id_asker AND id_asked = :id_asked";
	$parameters = array(':status' => 'Accepted', ':id_asker' => $friend_id, ':id_asked' => USER_ID);
	$stmt = ExecuteQuery($query, $parameters);
}
else
{
	$query = "DELETE FROM ".$_SESSION['Friends']." WHERE (id_asker = :my_id_asker AND id_asked = :friend_id_asked) OR (id_asker = :friend_id_asker AND id_asked = :my_id_asked)";
	$parameters = array(':my_id_asker' => USER_ID, ':friend_id_asked' => $friend_id, ':my_id_asked' => USER_ID, ':friend_id_asker' => $friend_id);
	$stmt = ExecuteQuery($query, $parameters);
}

//-----------------------------: SUCCESS/ERROR MESSAGE :--------------------------------------------
// DONE (if the request was previously sent so no need to check if it existed)
// Send the new friend list
include_once("Includes/Framework/GetFriends.php");

?>