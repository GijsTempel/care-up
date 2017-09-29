<?php
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	This script is an example of how to save, get and/or send files to the server
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// A screenshot has been sent so a lot of character could be truncated by anti injection system : let's do it manually
$decryptedData = read_unsafe($_POST["EncryptedInfo"]);		// Read encrypted data (decrypted with AES keys of the session)
$datas = explode(SEPARATOR, $decryptedData);

// Protect against injection for the message and encode in base64 the screenshot so all injection characters are encoded
$classicData1 = $datas[0];
$classicData2 = $datas[1];
$fileData = base64_encode($datas[2]);

$savegameName = "FileToSave";
// If the savegame exists -> update
if(checkSavegameExists(USER_ID, $savegameName))
{
	$query = "UPDATE ".$_SESSION['SaveGame']." SET file = :file WHERE account_id = :account_id AND name = :name";
	$parameters = array(':file' => $fileData,':account_id' => USER_ID,':name' => $savegameName);
	$stmt = ExecuteQuery($query, $parameters);
}
else
{
	$query = "INSERT INTO ".$_SESSION['SaveGame']." (account_id,name,file) VALUES (:account_id,:name,:file)";
	$parameters = array(':account_id' => USER_ID,':name' => $savegameName,':file' => $fileData);
	$stmt = ExecuteQuery($query, $parameters);
}


// Get the file back and send it to the client
$query = "SELECT file FROM ".$_SESSION['SaveGame']." WHERE account_id = :account_id AND name = :name";
$parameters = array(':account_id' => USER_ID,':name' => $savegameName);
$stmt = ExecuteQuery($query, $parameters);
$row = $stmt->fetch();

// SUCCESS
if(isset($row['file']))
{
	$fileData = $row["file"];
	$serverDatas = array(
			"File transfered",
			base64_decode($fileData)
	);
	sendArrayAndFinish($serverDatas);
}

end_script("Cannot get savegame back.");

?>