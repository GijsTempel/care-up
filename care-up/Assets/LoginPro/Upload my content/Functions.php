<?php

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : All the utility functions used in all PHP scripts
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////



define("SEPARATOR", "<DATA_SEPARATOR>", TRUE);
define("DELIMITOR", "<ENCRYPTED_DATA_DELIMITOR>", TRUE);



//																						UNIVERSAL
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function ExecuteQuery($query, $parameters)
{
	try
	{
		$stmt = $_SESSION['databaseConnection']->prepare($query);
		$stmt->execute($parameters);
		return $stmt;
	}
	catch (PDOException $e)
	{
		print "DATABASE ERROR: {" . $e->getMessage() . "}<br/>REQUEST:{".$query."}";
		die();
	}
}

// This function is very important -> USE IT TO DISPLAY ERROR MESSAGES (not encrypted)
function end_script($message)
{
	echo "ERROR: ".$message;
	// Close connection to database properly
	$_SESSION['databaseConnection'] = null;
	// Ensure the end of the current script
	die();
	exit(0);
}

// Random string generation
function generateRandomString($length = 30)
{
    $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    $charactersLength = strlen($characters);
    $randomString = '';
	// We can leave this function only if randomString is [$length] characters long
	while(strlen($randomString)<$length)
	{
		$randomString = '';
		for ($i=0; $i<$length; $i++)
		{
			$randomString .= $characters[rand(0, $charactersLength - 1)];
		}
	}
    return $randomString;
}

// Check if an array contains numbers only
function isArrayOfNumberOnly($array)
{
	$count = count($array);
	for($i=0; $i < $count; $i++)
	{
		if(!is_numeric($array[$i]))
			return false;
	}
	return true;
}

// Send email to a particular email address
/*
function sendMail($subject, $message, $receiverMail, $receiverUsername)
{
	mail($receiverMail,$subject,$message,$userheaders);
	return true;
	
	include_once('./Includes/Mail/PHPMailerAutoload.php');
	$mail = new PHPMailer(true); 	// The true param means it will throw exceptions on errors, which we need to catch
	$mail->IsSMTP(); 				// Telling the class to use SMTP

	try
	{
		$CompanyName = "MyGameCompanyName";
		$mail->IsSMTP(); // enable SMTP
		$mail->SMTPDebug = 0; // 0 = no message, 1 = errors and messages, 2 = messages only
		$mail->SMTPAuth = true; // authentication enabled
		$mail->SMTPSecure = 'tls';
		$mail->Host = "mail.redeagleunity.com";
		$mail->Port = 25;
		$mail->IsHTML(true);
		$mail->Username = $_SESSION['SERVER_email'];
		$mail->Password = $_SESSION['SERVER_emailPassword'];
		$mail->SetFrom($_SESSION['SERVER_email'], $CompanyName);
		$mail->FromName = $CompanyName;
		
		$mail->AddAddress($receiverMail);
		$mail->Subject = $subject;
		$mail->AddEmbeddedImage("./Includes/Mail/GameIconExample.png", "GameIcon", "./Includes/Mail/GameIconExample.png");
		$mail->Body = '<img src="cid:GameIcon"><br/><h3>'.$CompanyName.'</h3><br/>'.$message;
		$mail->IsHTML (true);
		
		$mail->Send();
		return true;
		//sendAndFinish("Email sent");
	}
	catch (phpmailerException $e)
	{
		// Error messages from PHPMailer
		end_script('Email sending failed: '.$e->errorMessage());
	}
	catch (Exception $e)
	{
		// Other error messages
		end_script('Email sending failed: '.$e->errorMessage());
	}
	return false;
}
*/



//																						AUTHENTIFICATION & REGISTRATION
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Authentification granted if the username exists, the password is correct AND the IP is not blocked for Connection attempts
function checkAuthentification($username, $password, $gameName, $IP, $session_token)
{
	$id = getUsernameID($username);														// Check account exists
	if($id == "") { end_script('This username is not linked to any account.'); }		// Authentification denied -> Redirect to authentification page
	
	if(SCAN_IP_ACTIVATED)																// If the IP scan is activated (in the installation process)
	{
		checkAttempts($id, $IP);														// Check if the maximum connection attempts is not reached
		checkIPIsValidated($id, $IP);													// Check if IP is validated for this account
		increaseAttempts($id, $IP, 'Connection');										// Increase connection attempts counter (fir this specified IP only and this action only)
	}
	
	$passwordIsValid = passwordVerification($username, $password);						// Check password
	if(!$passwordIsValid) { end_script('Incorrect password.'); }						// Authentification denied -> Redirect to authentification page
	
	if(isBanned($id)) { end_script('You have been banned by an administrator.'); }		// Check if account is banned or not
	
	$game = getGame($gameName);															// Check if the game exists and get its id
	if($game == null) { end_script('The game name you are trying to connect to is wrong : '.$gameName); }
	
	removeAttempts($id, $IP, 'Connection');												// Authentification granted : remove any unsuccessful previous attempts (for this IP and this account only)
	
	$accountIsActivated = checkAccountIsValidated($id);									// Check account activation
	if(!$accountIsActivated)
	{
		// If the account is not validated yet : send the account activation email
		sendAccountActivationEmail($id);
		end_script('Your account is not activated yet, please follow the link we sent you on your email address to activate it.');
	}
	
	// Success: let's save the session token in the account (to make sure only this user is connected with this account)
	$query = "UPDATE ".$_SESSION['AccountTable']." SET session_token = :session_token, current_game = :current_game WHERE username = :username";
	$parameters = array(':session_token' => $session_token,':current_game' => $game['id'],':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
}
function checkSessionToken($username, $session_token)
{
	$query = "SELECT session_token FROM ".$_SESSION['AccountTable']." WHERE username = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
	
	$datas = $stmt->fetch();
	if(isset($datas['session_token']))
	{
		return ($datas['session_token'] == $session_token);
	}
	return false;
}

// Check if information received are correct, check if mail and username are not already in use, then register the new user (+ create connection attempt : optional) finally : send the activation email
function register($mail, $username, $password, $IP)
{
	//---------------------------------: Are information valid ? :---------------------------------------
	if(!filter_var($mail, FILTER_VALIDATE_EMAIL)) { end_script('Registration failed, your email address is not valid.'); }
	if(strlen($username)<3) { end_script('Registration failed, your username is not valid.'); }
	if(strlen($password)<3) { end_script('Registration failed, your password is not valid.'); }
	
	//---------------------------------: Is account available ? :----------------------------------------
	if(checkMailExists($mail)) { end_script('Registration failed! This mail address is already in use.'); }
	if(checkUsernameExists($username)) { end_script('Registration failed, this username already exists.'); }
	
	// Add salt to the password and hash it
	$salt = generateRandomString(50);
	$password = hashPassword($password, $salt);
	
	//--------------------------------: Perform registration :-------------------------------------------
	// Generate validation_code
	$validation_code = generateRandomString(30);
	// Create a new account NOT activated
	$query = "INSERT INTO ".$_SESSION['AccountTable']." (id,mail,username,password,salt,validation_code,validated,creation_date,last_activity,last_connection_date) VALUES (NULL,:mail,:username,:password,:salt,:validation_code,:validated,NOW(),NOW(),NOW())";
	$parameters = array(':mail' => $mail,
						':username' => $username,
						':password' => $password,
						':salt' => $salt,
						':validation_code' => $validation_code,
						':validated' => 0);
	$stmt = ExecuteQuery($query, $parameters);
	
	//---------------------: Activate IP and send account activation email :-----------------------------
	$id = getUsernameID($username);			// Get the account ID
	createIPValidation($id, $IP, 1);		// Validate IP from where the registration has been completed
	sendAccountActivationEmail($id);		// Send account activation email
	return true;
}

function modify($currentUsername, $mail, $username, $password)
{
	// Get current information
	$account = getAccount($currentUsername);
	
	// Information to update
	if($mail!="" && $mail != $account['mail'])
	{
		if(checkMailExists($mail)) { end_script('This email address is already in use.'); } // Check if email address doesn't already exist
	}
	if($username!="" && $username != $account['username'])
	{
		if(checkUsernameExists($username)) { end_script('This username is already in use.'); } // Check if username doesn't already exist
	}
	if($password!="")
	{
		$salt = generateRandomString(50);
		$passwordHash = hashPassword($password,$salt);
	}
	
	$query = "UPDATE ".$_SESSION['AccountTable']." SET mail = :mail, username = :username, password = :password, salt = :salt WHERE id = :id";
	$parameters = array(':mail' => $mail,':username' => $username,':password' => $passwordHash,':salt' => $salt,':id' => $_SESSION['user_id']);
	$stmt = ExecuteQuery($query, $parameters);
	return true;
}

// Get user role
function getUserRole($username)
{
	$account = getAccount($username);
	if(count($account)==0) { end_script('Your account does not exists, please contact an administrator.'); }
	return $account['role'];
}






//																						GAME
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Get the user account
function getGame($gameName)
{
	$query = "SELECT * FROM ".$_SESSION['GameTable']." WHERE name COLLATE utf8_bin = :name";
	$parameters = array(':name' => $gameName);
	$stmt = ExecuteQuery($query, $parameters);
	
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return $row;
	}
	return null;
}
// Create or update (and set time played) a gaming session for the player
function connectGaming($gameId, $accountId)
{
	$query = "SELECT * FROM ".$_SESSION['GamingTable']." WHERE game_id = :game_id AND account_id = :account_id";
	$parameters = array(':game_id' => $gameId, ':account_id' => $accountId);
	$stmt = ExecuteQuery($query, $parameters);
	$gaming = $stmt->fetch();
	if(isset($gaming['game_id']))
	{
		// Update the time played of the last session
		$query = "UPDATE ".$_SESSION['GamingTable']." SET minutes_played_earlier = :minutes_played_earlier WHERE game_id = :game_id AND account_id = :account_id";
		$parameters = array(':minutes_played_earlier' => $gaming['minutes_played'], ':game_id' => $gameId, ':account_id' => $accountId);
		$stmt = ExecuteQuery($query, $parameters);
	}
	else
	{
		$query = "INSERT INTO ".$_SESSION['GamingTable']." (game_id,account_id) VALUES (:game_id,:account_id)";
		$parameters = array(':game_id' => $gameId, ':account_id' => $accountId);
		$stmt = ExecuteQuery($query, $parameters);
	}
}
// Get the gaming session of the player
function getGaming($gameId, $accountId)
{
	$query = "SELECT * FROM ".$_SESSION['GamingTable']." WHERE game_id = :game_id AND account_id = :account_id";
	$parameters = array(':game_id' => $gameId, ':account_id' => $accountId);
	$stmt = ExecuteQuery($query, $parameters);
	
	$row = $stmt->fetch();
	if(isset($row['game_id']))
	{
		return $row;
	}
	return null;
}



//																						NEWS
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Get the user account
function getNews($gameId)
{
	$query = "SELECT * FROM ".$_SESSION['NewsTable']." WHERE game_id = :game_id";
	$parameters = array(':game_id' => $gameId);
	$stmt = ExecuteQuery($query, $parameters);
	
	foreach($stmt as $row)
	{
		// Add new line
		if($news != "")
			$news = $news."\n\n";
		
		// Add new
		$news = $news."--------".$row["date"]." : ".$row["title"]."\n".$row["text"];
	}
	return $news;
}
// Add a new for the game
function addNew($gameId, $title, $text)
{
	$query = "INSERT INTO ".$_SESSION['NewsTable']." (id,game_id,date,title,text) VALUES (NULL,:game_id,NOW(),:title,:text)";
	$parameters = array(':game_id' => $gameId, ':title' => $title, ':text' => $text);
	$stmt = ExecuteQuery($query, $parameters);
}


//																						ACCOUNT
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Get the user account
function getAccount($username)
{
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE username COLLATE utf8_bin = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return $row;
	}
	return null;
}
// Get the user account
function getAccountById($id)
{
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE id = :id";
	$parameters = array(':id' => $id);
	$stmt = ExecuteQuery($query, $parameters);
	
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return $row;
	}
	return null;
}
// Get the user account
function getAccountByMail($mail)
{
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE mail = :mail";
	$parameters = array(':mail' => $mail);
	$stmt = ExecuteQuery($query, $parameters);
	
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return $row;
	}
	return null;
}

// Set the "validated" field of the account table to 1 (+ remove Validation attempts)
function activateAccount($id, $IP, $mail)
{
	$query = "UPDATE ".$_SESSION['AccountTable']." SET validated = 1 WHERE id = :id";
	$parameters = array(':id' => $id);
	$stmt = ExecuteQuery($query, $parameters);
	removeAttempts($id, $IP, 'Validation');
	removeAttempts($id, $IP, 'Resend');
	
	// Delete other accounts with the same email address that are not activated
	$query = "DELETE FROM ".$_SESSION['AccountTable']." WHERE mail = :mail AND validated = 0";
	$parameters = array(':mail' => $mail);
	$stmt = ExecuteQuery($query, $parameters);
	
	return true;
}

// Set the "validated" field of the account table to 1 (+ remove Validation attempts)
function checkAccountIsValidated($id)
{
	$datas = getAccountById($id);
	if(count($datas)==0) { end_script('Your account does not exists, please contact an administrator.'); }
	return $datas['validated']==1;
}

// Check if the account is banned or not
function isBanned($id)
{
	$datas = getAccountById($id);
	if(count($datas)==0) { end_script('Your account does not exists, please contact an administrator.'); }
	return $datas['banned']==1;
}

// Generate a new validation code for this account (useful for Account activation)
function generateNewAccountValidationCode($id)
{
	$validation_code = generateRandomString(30);
	
	$query = "UPDATE ".$_SESSION['AccountTable']." SET validation_code = :validation_code WHERE id = :id";
	$parameters = array(':validation_code' => $validation_code,':id' => $id);
	$stmt = ExecuteQuery($query, $parameters);
	
	return $validation_code;
}

// Generate a new validation code for this IP (useful for IP activation)
function generateNewIPValidationCode($id, $IP)
{
	$validation_code = generateRandomString(30);
	
	$query = "UPDATE ".$_SESSION['IPTable']." SET validation_code = :validation_code WHERE account_id = :account_id AND ip = :ip";
	$parameters = array(':validation_code' => $validation_code,':account_id' => $id,':ip' => $IP);
	$stmt = ExecuteQuery($query, $parameters);
	
	return $validation_code;
}
function updateConnectionDate($username)
{
	$query = "UPDATE ".$_SESSION['AccountTable']." SET last_connection_date = NOW() WHERE username = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
}
function updateActivity($username)
{
	$query = "UPDATE ".$_SESSION['AccountTable']." SET last_activity = NOW() WHERE username = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
}
function timeSinceLastActivity($username)
{
	$query = "SELECT last_activity, NOW() as now FROM ".$_SESSION['AccountTable']." WHERE username = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return strtotime($row['now']) - strtotime($row['last_activity']);
	}
	end_script("isConnected: last_activity not found.");
}
function isConnected($username)
{
	return timeSinceLastActivity($username) < TIMEOUT;
}



//																						PASSWORD
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Replace current VALIDATION_CODE (not the password !!) with a new one
// We don't replace the password because anybody with only the mail address can launch the action "Reinitialize password"
// The current password must be UNCHANGED (because the "real" owner of the account don't want his password changed all the time by usurpers)
// Then the validation code is sent via email to the specified email address
// If the link (with the validation code) is clicked in the email -> the action "sendPassword" is launch (which actually generate a new password and send it via email)
function reinitPassword($mail, $IP)
{
	//---------------------------------: Are information valid ? :---------------------------------------
	if(!filter_var($mail, FILTER_VALIDATE_EMAIL)) { end_script('Password reinitialization failed, your email address is not valid.'); }
	
	//-----------------------------------: Is account valid ? :------------------------------------------
	$account = getAccountByMail($mail);
	if(!isset($account['id'])) { end_script('Password reinitialization failed, this mail address is not linked to any account.'); }
	
	//--------------------------------------: Is IP valid ? :--------------------------------------------
	$id = $account['id'];
	checkForgotAttempts($id, $IP);
	increaseAttempts($id, $IP, 'Forgot');
	
	//----------------------------------------: Update :-------------------------------------------------
	$generatedValidationCode = generateRandomString(30);
	$query = "UPDATE ".$_SESSION['AccountTable']." SET validation_code = :validation_code WHERE mail = :mail";
	$parameters = array(':validation_code' => $generatedValidationCode,':mail' => $mail);
	$stmt = ExecuteQuery($query, $parameters);
	
	//----------------------------: Send email to confirm email address :--------------------------------
	$usersubject = "Your information to your account";
	$userheaders = "From: ".$_SESSION['SERVER_email']."\n";
	$usermessage = "If you want to change your password by a new generated one :\nPlease click the link below to change your password of your account \n\n  http://".$_SESSION['Domain']."/".$_SESSION['SecureLoginFolder']."/ReceiveNewPassword.php?mail=".$mail."&code=".$generatedValidationCode;
	mail($mail,$usersubject,$usermessage,$userheaders);
	return true;
}

// Generate a new password and send it via email to the user
function sendPassword($mail, $code, $IP)
{
	//---------------------------------: Are information valid ? :---------------------------------------
	if(!filter_var($mail, FILTER_VALIDATE_EMAIL)) { end_script('Password reinitialization failed, your email address is not valid.'); }
	if(strlen($code) < 10) { end_script('Password reinitialization failed, your code is not valid.'); }
	
	//-----------------------------------: Is account valid ? :------------------------------------------
	$account = getAccountByMail($mail);
	if(!isset($account['id'])) { end_script('Password reinitialization failed, this mail address is not linked to any account.'); }
	
	//--------------------------------------: Is IP valid ? :--------------------------------------------
	$id = $account['id'];
	checkForgotAttempts($id, $IP);
	
	$generatedPassword = generateRandomString(30);
	$salt = generateRandomString(50);
	$generatedPasswordHash = hashPassword(hash('sha256', $generatedPassword), $salt);
	
	// Get code
	if(!isset($account['id'])){ end_script('This email address is not linked to any account.'); }
	if($account['validation_code'] != $code) { end_script('Password reinitialization failed, your code is incorrect.'); }
	
	// Notice here : we have to hash the generated password with sha256 to match future sessions passwords received
	$query = "UPDATE ".$_SESSION['AccountTable']." SET password = :password, salt = :salt WHERE id = :id";
	$parameters = array(':password' => $generatedPasswordHash,':salt' => $salt,':id' => $id);
	$stmt = ExecuteQuery($query, $parameters);
	
	// Send email with generated password
	$usersubject = "Your information to your account";
	$userheaders = "From: ".$_SESSION['SERVER_email']."\n";
	$usermessage = "Your information to your account :\n - Username : ".$account['username']."\n - Password : ".$generatedPassword."\n\nYou can connect to your account with these information.\nCaution, even if nobody can connect to your account from an IP you didn't validate, keep your information in safe place.";
	mail($mail,$usersubject,$usermessage,$userheaders);
	
	// Remove attempts
	removeAttempts($id, $IP, 'Forgot');
	return true;
}

// Verify if password is valid for the specified username
function passwordVerification($CLIENT_username, $CLIENT_password)
{
	// Get the user account
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE username COLLATE utf8_bin = :username";
	$parameters = array(':username' => $CLIENT_username);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	
	if(isset($row['id']))
	{
		$passwordReceived = hashPassword($CLIENT_password, $row["salt"]);
		return $passwordReceived == $row["password"];
	}
	return false;
}



//																						FRIENDS
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Get the friend request
function getFriendRequest($user_id1, $user_id2)
{
	// Can't be the same username
	if($username1 == $username2)
		return null;
	
	$query = "SELECT * FROM ".$_SESSION['Friends']." WHERE (id_asker = :user_id1 OR id_asked = :user_id1) AND (id_asker = :user_id2 OR id_asked = :user_id2)";
	$parameters = array(':user_id1' => $user_id1, ':user_id2' => $user_id2);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['id_asker']))
	{
		return $row;
	}
	return null;
}



//																						IP
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Create IP validation (already activated or not?)
function createIPValidation($id, $IP, $activated = 0)
{
	$validation_code = generateRandomString(30);
	$query = "INSERT INTO ".$_SESSION['IPTable']." (account_id,ip,validation_code,validated,creation_date) VALUES (:account_id,:ip,:validation_code,:validated,NOW())";
	$parameters = array(':account_id' => $id,':ip' => $IP,':validation_code' => $validation_code,':validated' => $activated);
	$stmt = ExecuteQuery($query, $parameters);
}

// Get the IP information
function getIPInformation($id, $IP)
{
	$query = "SELECT * FROM ".$_SESSION['IPTable']." WHERE account_id = :account_id AND ip = :ip";
	$parameters = array(':account_id' => $id,':ip' => $IP);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	
	if(isset($row['account_id']))
	{
		return $row;
	}
	return null;
}

// Check if the specified IP is validated for this account (if not redirect to the IP validation page)
function checkIPIsValidated($id, $IP)
{
	$query = "SELECT * FROM ".$_SESSION['IPTable']." WHERE account_id = :account_id AND ip = :ip";
	$parameters = array(':account_id' => $id,':ip' => $IP);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	
	if(isset($row['account_id']))
	{
		// This IP is validated for this account -> connection with this IP granted
		if($row['validated']==1) { return true; }
	}
	else
	{
		// No IP connection found for this account : create one (NOT validated yet)
		createIPValidation($id, $IP, 0);
	}
	// We are here if :
	// 		- IP connection was found but not activated yet
	// 		- IP connection was NOT found (but we just created one above)
	
	// In the 2 cases : Redirect to the IP validation page
	// Send account activation email
	sendIPActivationEmail($id);
	end_script('Your IP is not activated for this account, please enter your IP password or follow the link we sent you on your email address.');
	return false;
}

// Set the "validated" field of the account table to 1 (+ remove Validation attempts)
function activateIP($id, $IP)
{
	$query = "UPDATE ".$_SESSION['IPTable']." SET validated = 1 WHERE account_id = :account_id AND ip = :ip";
	$parameters = array(':account_id' => $id,':ip' => $IP);
	$stmt = ExecuteQuery($query, $parameters);
	
	removeAttempts($id, $IP, 'IP Validation');
	removeAttempts($id, $IP, 'Resend IP');
	return true;
}


//																						ATTEMPTS
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Create an attempt for a specified action (for only one pair of (id,IP))
function createAttempt($id, $IP, $action)
{
	$query = "INSERT INTO ".$_SESSION['AttemptsTable']." (account_id,ip,action,attempts) VALUES (:account_id,:ip,:action,1)";
	$parameters = array(':account_id' => $id,':ip' => $IP,':action' => $action);
	$stmt = ExecuteQuery($query, $parameters);
}

// Get an attempt for a specified action (for only one pair of (id,IP))
function getAttempts($id, $IP, $action)
{
	$query = "SELECT * FROM ".$_SESSION['AttemptsTable']." WHERE account_id = :account_id AND ip = :ip AND action = :action";
	$parameters = array(':account_id' => $id,':ip' => $IP,':action' => $action);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	
	if(isset($row['account_id']))
	{
		return $row['attempts'];
	}
	return 0;
}

// Set an attempt for a specified action (for only one pair of (id,IP))
function setAttempts($id, $IP, $action, $attempts)
{
	$query = "UPDATE ".$_SESSION['AttemptsTable']." SET attempts = :attempts WHERE account_id = :account_id AND ip = :ip AND action = :action";
	$parameters = array(':attempts' => $attempts,':account_id' => $id,':ip' => $IP,':action' => $action);
	$stmt = ExecuteQuery($query, $parameters);
}

// Remove all attempts for a specified action (for only one pair of (id,IP))
function removeAttempts($id, $IP, $action)
{
	$query = "DELETE FROM ".$_SESSION['AttemptsTable']." WHERE account_id = :account_id AND ip = :ip AND action = :action";
	$parameters = array(':account_id' => $id,':ip' => $IP,':action' => $action);
	$stmt = ExecuteQuery($query, $parameters);
}

// Increment an attempt for a specified action (for only one pair of (id,IP)) -> if no attempt exists, create one
function increaseAttempts($id, $IP, $action)
{
	$attempts = getAttempts($id, $IP, $action) + 1;
	if($attempts == 1) { createAttempt($id, $IP, $action); }
	else { setAttempts($id, $IP, $action, $attempts); }
}

// Check if the IP hasn't reach the maximum connection attempts yet (for the specified account)
function checkAttempts($id, $IP)
{
	$attempts = getAttempts($id, $IP, 'Connection');
	if($attempts >= $_SESSION['AvailableAttemptsBeforeBlocking']) { end_script('Your IP is blocked for this account, please contact an administrator.'); }
}

// Check if the IP hasn't reach the maximum "Resend" attempts yet (for the specified account)
function checkResendAttempts($id, $IP)
{
	$attempts = getAttempts($id, $IP, 'Resend');
	if($attempts >= $_SESSION['AvailableAttemptsBeforeBlocking']) { end_script('You cannot send account validation email anymore, please contact an administrator.'); }
}

// Check if the IP hasn't reach the maximum "Resend" attempts yet (for the specified account)
function checkResendIPAttempts($id, $IP)
{
	$attempts = getAttempts($id, $IP, 'Resend IP');
	if($attempts >= $_SESSION['AvailableAttemptsBeforeBlocking']) { end_script('You cannot send IP validation email anymore, please contact an administrator.'); }
}

// Check if the IP hasn't reach the maximum "Validation" attempts yet (for the specified account)
function checkAccountValidationAttempts($id, $IP)
{
	$attempts = getAttempts($id, $IP, 'Validation');
	if($attempts >= $_SESSION['AvailableAttemptsBeforeBlocking']) { end_script('You cannot activate your account from your IP address anymore, please contact an administrator.'); }
}

// Check if the IP hasn't reach the maximum "Validation" attempts yet (for the specified account)
function checkIPValidationAttempts($id, $IP)
{
	$attempts = getAttempts($id, $IP, 'IP Validation');
	if($attempts >= $_SESSION['AvailableAttemptsBeforeBlocking']) { end_script('You cannot activate your IP for this account anymore, please contact an administrator.'); }
}

// Check if the IP hasn't reach the maximum "Forgot" attempts yet (for the specified account)
function checkForgotAttempts($id, $IP)
{
	$attempts = getAttempts($id, $IP, 'Forgot');
	if($attempts >= $_SESSION['AvailableAttemptsBeforeBlocking']) { end_script('You cannot get your information back from this IP anymore, please contact an administrator.'); }
}



//																					ACHIEVEMENTS
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Unlock an achievement for a user (if percent > 0)
// Lock an achievement for a user (if percent == 0) In case an achievement is unlocked with a percentage for a moment, and the player didn't succeed the mission in the time left : lock it again
function unlockAchievement($account_id, $achievementName, $percent)
{
	if($percent > 0)
	{
		$query = "SELECT * FROM ".$_SESSION['AchievementsTable']." WHERE account_id = :account_id AND name = :name";
		$parameters = array(':account_id' => $account_id, ':name' => $achievementName);
		$stmt = ExecuteQuery($query, $parameters);
		$row = $stmt->fetch();
		if(isset($row['account_id']))
		{
			// Update the time played of the last session
			$query = "UPDATE ".$_SESSION['AchievementsTable']." SET percent = :percent, date = NOW() WHERE account_id = :account_id AND name = :name";
			$parameters = array(':percent' => $percent, ':account_id' => $account_id, ':name' => $achievementName);
			$stmt = ExecuteQuery($query, $parameters);
		}
		else
		{
			$query = "INSERT INTO ".$_SESSION['AchievementsTable']." (account_id,name,percent,date) VALUES (:account_id,:name,:percent,NOW())";
			$parameters = array(':account_id' => $account_id, ':name' => $achievementName, ':percent' => $percent);
			$stmt = ExecuteQuery($query, $parameters);
		}
	}
	else if($percent == 0)
	{
		$query = "DELETE FROM ".$_SESSION['AchievementsTable']." WHERE account_id = :account_id AND name = :name";
		$parameters = array(':account_id' => $account_id, ':name' => $achievementName);
		$stmt = ExecuteQuery($query, $parameters);
	}
}



//																						EMAILS
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Send (or resend) the account activation email for an account
function sendAccountActivationEmail($id)
{
	//-------------------------------: Send email to confirm email address :------------------------------
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE id = :id";
	$parameters = array(':id' => $id);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(!isset($row['id'])) { end_script('Email activation not sent, the ID is not linked to any account.'); }
	
	//-------------------------------------: Check resend attempts :--------------------------------------
	$IP = $_SERVER['REMOTE_ADDR'];
	checkResendAttempts($id, $IP);
	increaseAttempts($id, $IP, 'Resend');
	
	//----------------------------------------: Get needed data :-----------------------------------------
	$mail = $row['mail'];
	$username = $row['username'];
	
	//--------------------------------: Generated a new validation code :---------------------------------
	$validation_code = generateNewAccountValidationCode($id);
	
	//----------------------------------: Send the email validation :-------------------------------------
	$usermessage = "Please click the link below to confirm you email address in order to activate your account \n http://".$_SESSION['Domain']."/".$_SESSION['SecureLoginFolder']."/ValidateAccountByURL.php?username=".$username."&code=".$validation_code;
	$usersubject = "Confirm your email address (".$mail.") to activate your account (".$username.")";
	$userheaders = "From: ".$_SESSION['SERVER_email']."\n";
	mail($mail,$usersubject,$usermessage,$userheaders);
	
	return true;
}

// Send (or resend) the IP activation email
function sendIPActivationEmail($id)
{
	//-------------------------------: Send email to confirm email address :------------------------------
	$account = getAccountById($id);
	if(!isset($account['id'])) { end_script('Email activation not sent, the ID is not linked to any account.'); }
	
	//-------------------------------------: Check resend attempts :--------------------------------------
	$IP = $_SERVER['REMOTE_ADDR'];
	checkResendIPAttempts($id, $IP);
	increaseAttempts($id, $IP, 'Resend IP');
	
	//----------------------------------------: Get needed data :-----------------------------------------
	$mail = $account['mail'];
	$username = $account['username'];
	
	//--------------------------------: Generated a new validation code :---------------------------------
	$validation_code = generateNewIPValidationCode($id, $IP);
	
	//----------------------------------: Send the email validation :-------------------------------------
	$usermessage = "Please click the link below to confirm you email address in order to activate your IP for your account \n http://".$_SESSION['Domain']."/".$_SESSION['SecureLoginFolder']."/ValidateIPByURL.php?username=".$username."&code=".$validation_code;
	$usersubject = "Confirm your IP via your email address (".$mail.") to activate your account (".$username.") from this IP";
	$userheaders = "From: ".$_SESSION['SERVER_email']."\n";
	mail($mail,$usersubject,$usermessage,$userheaders);
	return true;
}




//																						EXISTS ?
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Check if the email address is already in use
function checkMailExists($mail)
{
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE mail = :mail AND validated = 1";
	$parameters = array(':mail' => $mail);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	return isset($row['id']);
}

// Check if the username is already in use
function checkUsernameExists($username)
{
	$query = "SELECT * FROM ".$_SESSION['AccountTable']." WHERE username COLLATE utf8_bin = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	return isset($row['id']);
}

// Check if the savegame already exists for this user
function checkSavegameExists($account_id, $name)
{
	$query = "SELECT name FROM ".$_SESSION['SaveGame']." WHERE account_id = :account_id AND name = :name";
	$parameters = array(':account_id' => $account_id,':name' => $name);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	return isset($row['name']);
}

// Check if the server exists
function checkServerExists($serverId)
{
	$query = "SELECT id FROM ".$_SESSION['Server']." WHERE id = :id";
	$parameters = array(':id' => $serverId);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	return isset($row['id']);
}



//																						GET UNIQUE DATA FROM ANOTHER
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Get the account ID
function getUsernameID($username)
{
	$query = "SELECT id FROM ".$_SESSION['AccountTable']." WHERE username COLLATE utf8_bin = :username";
	$parameters = array(':username' => $username);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return $row['id'];
	}
	return "";
}

// Get the username from the email address
function getUsernameFromMail($mail)
{
	// Get the user account
	$query = "SELECT username FROM ".$_SESSION['AccountTable']." WHERE mail = :mail";
	$parameters = array(':mail' => $mail);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['username']))
	{
		return $row['username'];
	}
	return "";
}

// Get the username from the id
function getUsernameFromId($id)
{
	$query = "SELECT username FROM ".$_SESSION['AccountTable']." WHERE id = :id";
	$parameters = array(':id' => $id);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['username']))
	{
		return $row['username'];
	}
	return "";
}

// Get the account ID from the email address
function getIdFromMail($mail)
{
	// Get the user account
	$query = "SELECT id FROM ".$_SESSION['AccountTable']." WHERE mail = :mail";
	$parameters = array(':mail' => $mail);
	$stmt = ExecuteQuery($query, $parameters);
	$row = $stmt->fetch();
	if(isset($row['id']))
	{
		return $row['id'];
	}
	return "";
}




//																						AES DECRYPTION
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Useful to get data from POST (since everything is AES encrypted we can use it every time we access POST array
function read($data)
{
	return aes_decrypt(base64_decode($data));
}
// Here to prevent SQL injection, we don't replace characters, we encode the string in Base64
// Use it for the data with special characters and "forbidden" strings (like select, insert, all special characters and everything)
function read_SpecialData($data)
{
	return aes_decrypt(base64_decode($data), true);
}
// Use this function only if you protect against injection afterwards
function read_unsafe($EncryptedData)
{
	$keyString = $_SESSION['aes_key'];
	$ivString = $_SESSION['aes_iv'];
	
	$decrypted_value = strippadding(mcrypt_decrypt(MCRYPT_RIJNDAEL_256, $keyString, base64_decode($EncryptedData), MCRYPT_MODE_CBC, $ivString));
	$splits = explode('<ENC_END>', $decrypted_value);
	
	// If there isn't exactly 2 members in the AES_KEYS string -> leave
	if(count($splits)!=2) { echo('aes_decrypt function : decrypted_value malformed.'); exit(0); }
	
	return $splits[0];
}

// AES encryption
function aes_encrypt($string)
{
	$keyString = $_SESSION['aes_key'];
	$ivString = $_SESSION['aes_iv'];
	
	$toBeEncrypted = $string.'<ENC_END>';
	return base64_encode(mcrypt_encrypt(MCRYPT_RIJNDAEL_256, $keyString, addpadding($toBeEncrypted), MCRYPT_MODE_CBC, $ivString));
}
function addpadding($string, $blocksize = 32)
{
    $len = strlen($string);
    $pad = $blocksize - ($len % $blocksize);
    $string .= str_repeat(chr($pad), $pad);
    return $string;
}

// AES decryption
function aes_decrypt($EncryptedData, $fileDecryption=false)
{
	$keyString = $_SESSION['aes_key'];
	$ivString = $_SESSION['aes_iv'];
	
	$decrypted_value = strippadding(mcrypt_decrypt(MCRYPT_RIJNDAEL_256, $keyString, $EncryptedData, MCRYPT_MODE_CBC, $ivString));
	//$decrypted_value = mcrypt_decrypt(MCRYPT_RIJNDAEL_256, $keyString, $EncryptedData, MCRYPT_MODE_CBC, $ivString);
	$splits = explode('<ENC_END>', $decrypted_value);
	// If there isn't exactly 2 members in the AES_KEYS string -> leave
	if(count($splits)!=2) { echo('aes_decrypt function : decrypted_value malformed.'); exit(0); }
	
	if($fileDecryption)
	{
		// If we are decrypting a file : Here to prevent SQL injection, we don't replace characters, we encode the string in Base64
		return base64_encode($splits[0]);
	}
	return $splits[0];
}
function strippadding($string)
{
    $slast = ord(substr($string, -1));
    $slastc = chr($slast);
    $pcheck = substr($string, -$slast);
    if(preg_match("/$slastc{".$slast."}/", $string))
	{
        $string = substr($string, 0, strlen($string)-$slast);
        return $string;
    }
	return false;
}

// Send information to the game WITH AES encryption
function sendAndFinish($message)
{
	// Send message AES encrypted with session token at the end
	echo DELIMITOR.aes_encrypt($message.SEPARATOR.$_SESSION['session_token']).DELIMITOR;
	// Close connection to database properly (if exists)
	$_SESSION['databaseConnection'] = null;
	// Ensure the end of the current script
	die();
	exit(0);
}
// Send information to the game WITH AES encryption
function sendArrayAndFinish($datas)
{
	// Ensure $datas is not empty
	if(count($datas)<=0) { end_script("sendArrayAndFinish: $datas array is empty"); }
	
	$message = $datas[0];
	for($x=1; $x<count($datas); $x++)
	{
		$message = $message.SEPARATOR.$datas[$x];
	}
	sendAndFinish($message);
}
// Send information to the game WITH AES encryption
function send_SpecialData($message)
{
	echo DELIMITOR.aes_encrypt(base64_decode($message)).DELIMITOR;
}
// Send information to the game WITHOUT AES encryption
function sendDatas_free($message)
{
	echo DELIMITOR.$message.DELIMITOR;
}

// Add salt and hash
function hashPassword($password, $salt)
{
	return hash('sha256', $password.$salt);
}






?>