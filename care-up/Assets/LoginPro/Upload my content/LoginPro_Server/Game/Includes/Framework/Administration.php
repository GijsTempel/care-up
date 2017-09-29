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
	end_script("Administration: Only administrators can have access to administration : role = ".$_SESSION['role']);


$query = "SELECT ".$_SESSION['Report'].".id as id, ".$_SESSION['Report'].".creation_date as creation_date, ".$_SESSION['Report'].".done_date as done_date, ".$_SESSION['Report'].".message as message, ".$_SESSION['Report'].".screenshot as screenshot, ".$_SESSION['AccountTable'].".username as username FROM ".$_SESSION['Report']." JOIN ".$_SESSION['AccountTable']." ON reporter_id = ".$_SESSION['AccountTable'].".id";
$parameters = array();
$stmt = ExecuteQuery($query, $parameters);

// SUCCESS
$datasToSend = array();
$noReportFound = true;
foreach($stmt as $row)
{
	// If it's the first report of the list : message = reports found
	if($noReportFound)
	{
		$noReportFound = false;
		$datasToSend[] = "Report list received.";
	}
	
	$datasToSend[] = $row["id"];						// The id of the report (NOT the reporter, the report)
	$datasToSend[] = $row["creation_date"];				// The date of the report
	$datasToSend[] = $row["username"];					// The reporter username
	$datasToSend[] = $row["message"];					// The reporter message
	// We don't send the screenshot so the loading is lazy (loaded only on demand of each screenshot) otherwise it takes too long to get all of them at once
	//$datasToSend[] = $row["screenshot"];				// The screenshot image
	$datasToSend[] = $row["done_date"]==null ? "False" : "True";	// Done if the doneDate is not null
}

// If no achievement exists : message = no achievement
if($noReportFound)
{
	$datasToSend[] = "No report to display.";
}

sendArrayAndFinish($datasToSend);

?>