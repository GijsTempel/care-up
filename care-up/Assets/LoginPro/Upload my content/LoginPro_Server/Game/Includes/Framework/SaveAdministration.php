<?php
// Here we check if we are called from 'Server.php' script : in any other cases WE LEAVE !
if(!isset($ServerScriptCalled)) { exit(0); }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	This script allow administrators to make changes on report like remove some of them or check them as "done"
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Verify administrator
if(!isset($_SESSION['role']) || $_SESSION['role'] != 'Admin')
	end_script("SaveAdministration: Only administrators can have access to administration.");

// Get all the reports to delete
$toDelete = array();
$toFlagDone = array();
$toFlagNotDone = array();

$datasCount = count($datas);
for($i=0; ($i+2) < $datasCount; $i+=3)
{
	if($datas[$i+2] == "True")
		$toDelete[] = $datas[$i];		// The report must be deleted
	else if($datas[$i+1] == "True")
		$toFlagDone[] = $datas[$i];		// The report flag "Done" must be True
	else
		$toFlagNotDone[] = $datas[$i];	// The report flag "Done" must be False
}

// Protect against injection by checking if all datas are numbers only
if(!isArrayOfNumberOnly($toDelete)) { end_script("SaveAdministration: Only ids of reports can be specified in toDelete array."); }
if(!isArrayOfNumberOnly($toFlagDone)) { end_script("SaveAdministration: Only flags for reports can be specified in toFlagDone array."); }
if(!isArrayOfNumberOnly($toFlagNotDone)) { end_script("SaveAdministration: Only flags for reports can be specified in toFlagNotDone array."); }

// DELETE
$deleteRequest = "DELETE FROM ".$_SESSION['Report']." WHERE id IN (";
$toDeleteCount = count($toDelete);
for($i=0; $i < ($toDeleteCount-1); $i++)
{
	$deleteRequest .= $toDelete[$i].",";
}
if($toDeleteCount > 0)
	$deleteRequest .= $toDelete[$toDeleteCount-1];
else
	$deleteRequest .= "0";
$deleteRequest .= ")";

// DONE = True
$toFlagDoneRequest = "UPDATE ".$_SESSION['Report']." SET done_date=NOW() WHERE done_date IS NULL AND id IN (";
$toFlagDoneCount = count($toFlagDone);
for($i=0; $i < ($toFlagDoneCount-1); $i++)
{
	$toFlagDoneRequest .= $toFlagDone[$i].",";
}
if($toFlagDoneCount > 0)
	$toFlagDoneRequest .= $toFlagDone[$toFlagDoneCount-1];
else
	$toFlagDoneRequest .= "0";
$toFlagDoneRequest .= ")";

// DONE = False
$toFlagNotDoneRequest = "UPDATE ".$_SESSION['Report']." SET done_date=NULL WHERE id IN (";
$toFlagNotDoneCount = count($toFlagNotDone);
for($i=0; $i < ($toFlagNotDoneCount-1); $i++)
{
	$toFlagNotDoneRequest .= $toFlagNotDone[$i].",";
}
if($toFlagNotDoneCount > 0)
	$toFlagNotDoneRequest .= $toFlagNotDone[$toFlagNotDoneCount-1];
else
	$toFlagNotDoneRequest .= "0";
$toFlagNotDoneRequest .= ")";

// Execute requests
$stmt = ExecuteQuery($deleteRequest, array());
$stmt = ExecuteQuery($toFlagDoneRequest, array());
$stmt = ExecuteQuery($toFlagNotDoneRequest, array());

// SUCCESS
sendAndFinish("Report list saved.");

?>