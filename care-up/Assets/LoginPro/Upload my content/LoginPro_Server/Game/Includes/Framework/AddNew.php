<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

//////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Send the news of the game specified at startup
//
//////////////////////////////////////////////////////////////////////////////////////////


// Check data presence of the new
$title = $datas[0];
$text = $datas[1];
if(!isset($title)) { end_script("AddNew: title not set."); }
if(!isset($text)) { end_script("AddNew: text not set."); }

// Add the new
addNew($account['current_game'], $title, $text);

// Return all news to be displayed
sendAndFinish("New added.");

?>