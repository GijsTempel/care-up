<?php

$scene  = $datas[0];
$number = $datas[1];

$query = "SELECT ".$_SESSION['AccountTable'].".username, ".$_SESSION['HighScores'].".score, ".$_SESSION['HighScores'].".time FROM ".$_SESSION['HighScores']." INNER JOIN ".$_SESSION['AccountTable']." ON ".$_SESSION['HighScores'].".account_id = ".$_SESSION['AccountTable'].".id WHERE scene = :scene ORDER BY score DESC LIMIT :number";
$parameters = array(':scene' => $scene, ':number' => $number);
$stmt = ExecuteQuery($query, $parameters);

$dataToGetFromServer = array();

while ($row = $stmt->fetch())
{
	$dataToGetFromServer[] = $row["username"];
	$dataToGetFromServer[] = $row["score"];
	$dataToGetFromServer[] = $row["time"];
}

$dataToGetFromServer[] = "yay, it's not empty now, quit throwing errors";
sendArrayAndFinish($dataToGetFromServer);

?>