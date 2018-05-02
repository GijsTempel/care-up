<?php

$scene = $datas[0];
$score = $datas[1];
$time  = $datas[2];

$query = "SELECT * FROM ".$_SESSION['HighScores']." WHERE account_id = :id AND scene = :scene";
$parameters = array(':id' => $account['id'], ':scene' => $scene);
$stmt = ExecuteQuery($query, $parameters);

$info = $stmt->fetch();

if(isset($info['score'])) // if already exists
{
	// we compare if new score is better
	if ($info['score'] < $score)
	{
		// update it on database
		$query = "UPDATE ".$_SESSION['HighScores']." Set score = :score, time = :time WHERE account_id = :id AND scene = :scene";
		$parameters = array(':score' => $score, ':time' => $time, ':id' => $account['id'], ':scene' => $scene);
		$stmt = ExecuteQuery($query, $parameters);
		
		// we done
		sendAndFinish("HishScore updated.");
	}
	else
	{
		// do nothing, end script
		sendAndFinish("There is already better score.");
	}
}
else // there was no high score, let's add one
{
	$query = "INSERT INTO ".$_SESSION['HighScores']." (account_id, scene, score, time) VALUES (:id, :scene, :score, :time)";
	$parameters = array(':score' => $score, ':time' => $time, ':id' => $account['id'], ':scene' => $scene);
	$stmt = ExecuteQuery($query, $parameters);
	
	// we done
	sendAndFinish("HishScore created.");
}

end_script("Something went wrong");

?>