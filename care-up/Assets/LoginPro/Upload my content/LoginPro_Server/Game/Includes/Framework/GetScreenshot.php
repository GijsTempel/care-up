<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	The script send all reports contained in the report table to be treated by administrators
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Verify administrator
if(!isset($_SESSION['role']) || $_SESSION['role'] != 'Admin')
	end_script("GetScreenshot: Only administrators can have access to administration");

// Verify we received the reportId
$reportId = $datas[0];
if(!isset($reportId) || $reportId == "")
	end_script("GetScreenshot: reportId not received.");

$query = "SELECT screenshot FROM ".$_SESSION['Report']." WHERE id = :id";
$parameters = array(':id' => $reportId);
$stmt = ExecuteQuery($query, $parameters);
$row = $stmt->fetch();

// SUCCESS
if(isset($row['screenshot']))
{
	$datasToSend = array();
	$datasToSend[] = "Screenshot received.";
	$datasToSend[] = base64_decode($row["screenshot"]);	// We decode the screenshot from base64 string because it had been saved encoded in base 64 string
	
	sendArrayAndFinish($datasToSend);
}

end_script("GetScreenshot: no report found for the id = ".$reportId);

?>