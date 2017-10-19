<?php
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	This script is used to allow players to report abuse in game
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// A screenshot has been sent so a lot of character could be truncated by anti injection system : let's do it manually
$decryptedData = read_unsafe($_POST["EncryptedInfo"]);		// Read encrypted data (decrypted with AES keys of the session)
$datas = explode(SEPARATOR, $decryptedData);

// Protect against injection for the message and encode in base64 the screenshot so all injection characters are encoded
$message = $datas[0];
$screenshot = base64_encode($datas[1]);

// If the savegame exists -> update
if(isset($message) && isset($screenshot))
{
	$query = "INSERT INTO ".$_SESSION['Report']." (creation_date,reporter_id,message,screenshot) VALUES (NOW(),:reporter_id,:message,:screenshot)";
	$parameters = array(':reporter_id' => USER_ID,':message' => $message,':screenshot' => $screenshot);
	$stmt = ExecuteQuery($query, $parameters);
	
	$serverDatas = array(
		"Abuse reported, an administrator will study the case."
	);
	sendArrayAndFinish($serverDatas);
}

end_script("Unable to save the report.");

?>