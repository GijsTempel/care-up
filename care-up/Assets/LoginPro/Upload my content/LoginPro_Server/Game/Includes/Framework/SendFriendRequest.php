<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Check authentification, if correct : send friend request
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//------------------------------: GET REQUEST INFORMATION :---------------------------------------------
$friend_username = $datas[0];

//---------------------------------: VERIFICATION :--------------------------------------------------
if($friend_username!="" && strlen($friend_username)<3) { end_script('The friend name ust be at least 3 characters long.'); }
if($friend_username==USERNAME) { end_script('Do not ask yourself.'); }

//------------------------------: UPDATE INFORMATION :-----------------------------------------------

// Get friend information
$friend_account = getAccount($friend_username);

// Check if username exists
if(!isset($friend_account['id'])) { end_script('Sorry, this username does not exist.'); }

// Check if friend request has already been sent or not
$friendRequest = getFriendRequest(USER_ID, $friend_account['id']);

// Check if the friend request has already been sent to this player
if(isset($friendRequest['id'])) { end_script('The friend request has already been sent to this player.'); }

// Create a new friend request
$query = "INSERT INTO ".$_SESSION['Friends']." (id_asker,id_asked,status) VALUES (:id_asker,:id_asked,:status)";
$parameters = array(':id_asker' => USER_ID,
					':id_asked' => $friend_account['id'],
					':status' => 'Pending');
$stmt = ExecuteQuery($query, $parameters);

//-----------------------------: SUCCESS/ERROR MESSAGE :--------------------------------------------
sendAndFinish("Friend request sent !");

?>