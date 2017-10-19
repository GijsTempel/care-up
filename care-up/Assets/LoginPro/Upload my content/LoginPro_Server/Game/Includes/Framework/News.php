<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

//////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Send the news of the game specified at startup
//
//////////////////////////////////////////////////////////////////////////////////////////

$game = getGame($gameName);
if($game == null)
{ end_script('News: game name invalid :'.$gameName); }

sendAndFinish(getNews($game['id']));

?>