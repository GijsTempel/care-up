<?php

$scene = $datas[0];

$query = "SELECT * FROM ".$_SESSION['HighScores']." WHERE account_id = :id AND scene = :scene";
$parameters = array(':id' => $account['id'], ':scene' => $scene);
$stmt = ExecuteQuery($query, $parameters);

$dataToGetFromServer = array();
$dataToGetFromServer[] = "fill";

$info = $stmt->fetch();

if(isset($info['score']))
{
	$dataToGetFromServer[] = $info["score"];
	$dataToGetFromServer[] = $info["time"];
}

sendArrayAndFinish($dataToGetFromServer);

?>