<?php

if(isset($_POST['Action'])) { $ACTION = $_POST['Action']; }
if($ACTION != "CheckDatabase") { echo "The action is not set to 'CheckDatabase'."; }

// Check if information has been received correctly
if(!isset($_POST['Host']) || strlen($_POST['Host'])==0) { echo "ERROR : Host is missing !"; exit(0); }
if(!isset($_POST['Name']) || strlen($_POST['Name'])==0) { echo "ERROR : Name is missing !"; exit(0); }
if(!isset($_POST['User']) || strlen($_POST['User'])==0) { echo "ERROR : User is missing !"; exit(0); }
if(!isset($_POST['Password']) || strlen($_POST['Password'])==0) { echo "ERROR : Password is missing !"; exit(0); }
if(!isset($_POST['GameName']) || strlen($_POST['GameName'])==0) { echo "ERROR : GameName is missing !"; exit(0); }
if(!isset($_POST['GameVersion']) || strlen($_POST['GameVersion'])==0) { echo "ERROR : GameVersion is missing !"; exit(0); }

$host = $_POST['Host'];
$name = $_POST['Name'];
$user = $_POST['User'];
$password = $_POST['Password'];
// The both variables below are just used when the tables are created
$gameToCreate = $_POST['GameName'];
$gameVersionToCreate = $_POST['GameVersion'];


// ------------------ Check Database existence
$databaseConnection = new mysqli($host, $user, $password, $name);
$request = "SELECT table_name FROM information_schema.tables";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : The database doesn't exists (or not reachable), or the user you used doesn't have privileges to execute requests : ".mysqli_error($databaseConnection));


// ------------------ Create tables if not exist
// ACCOUNT
$existenceQuery = "SHOW TABLES LIKE 'Account'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Account (
  id bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  mail varchar(255) NOT NULL,
  username varchar(255) NOT NULL,
  password text NOT NULL,
  salt varchar(255) NOT NULL,
  role varchar(255) NOT NULL DEFAULT 'Player',
  validation_code varchar(255) NOT NULL,
  validated int(11) NOT NULL,
  banned int(1) NOT NULL DEFAULT '0',
  creation_date datetime NOT NULL,
  session_token varchar(255) DEFAULT NULL,
  last_activity datetime NOT NULL,
  last_connection_date datetime NOT NULL,
  current_game bigint(20) unsigned DEFAULT NULL,
  joined_server_id bigint(20) DEFAULT NULL,
  score int(255) NOT NULL DEFAULT '0',
  PRIMARY KEY (id),
  UNIQUE KEY username (username)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Players (admin, moderators, ...) accounts are saved here' AUTO_INCREMENT=1";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING ACCOUNT TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Account table exists
$request = "SELECT * FROM Account";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Account table is missing.");


// ACHIEVEMENTS
$existenceQuery = "SHOW TABLES LIKE 'Achievements'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Achievements (
  account_id bigint(20) unsigned NOT NULL,
  name varchar(255) NOT NULL,
  percent int(3) NOT NULL DEFAULT '100',
  date date NOT NULL,
  PRIMARY KEY (account_id,name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='All unlocked achievements by player'";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING ACHIEVEMENTS TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Achievements table exists
$request = "SELECT * FROM Achievements";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Achievements table is missing.");

// ATTEMPTS
$existenceQuery = "SHOW TABLES LIKE 'Attempts'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Attempts (
  account_id int(11) unsigned NOT NULL,
  ip varchar(255) NOT NULL,
  action varchar(255) NOT NULL,
  attempts int(11) NOT NULL,
  PRIMARY KEY (account_id,ip,action)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Number of attempts recorded for any action'";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING ATTEMPTS TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Attempts table exists
$request = "SELECT * FROM Attempts";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Attempts table is missing.");

// GAME
$existenceQuery = "SHOW TABLES LIKE 'Game'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Game (
  id bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  name varchar(255) NOT NULL,
  version varchar(255) NOT NULL DEFAULT '0.1',
  creation_date date NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY name (name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='The name of your games' AUTO_INCREMENT=1";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING GAME TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Game table exists
$request = "SELECT * FROM Game";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Game table is missing.");

// ------------------ Insert the game if no entry
if(mysqli_num_rows($results) == 0)
{
	$SQLinsert = "INSERT INTO Game (id,name,version,creation_date) VALUES (NULL,'".$gameToCreate."','".$gameVersionToCreate."',NOW())";
	mysqli_query($databaseConnection, $SQLinsert) or die("CREATE GAMING DATABASE ERROR : ".$SQLinsert."; ERROR =  ".mysqli_error($databaseConnection));
}

// GAMING
$existenceQuery = "SHOW TABLES LIKE 'Gaming'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Gaming (
  game_id bigint(20) unsigned NOT NULL,
  account_id bigint(20) unsigned NOT NULL,
  minutes_played_earlier int(255) NOT NULL DEFAULT '0',
  minutes_played int(255) NOT NULL DEFAULT '0',
  data1 varchar(255) DEFAULT NULL,
  data2 varchar(255) DEFAULT NULL,
  data3 varchar(255) DEFAULT NULL,
  PRIMARY KEY (game_id,account_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='All game information by player'";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING GAMING TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Gaming table exists
$request = "SELECT * FROM Gaming";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Gaming table is missing.");

// IP
$existenceQuery = "SHOW TABLES LIKE 'IP'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS IP (
  account_id bigint(20) unsigned NOT NULL,
  ip varchar(255) NOT NULL,
  validation_code varchar(255) NOT NULL,
  validated int(11) NOT NULL,
  creation_date date NOT NULL,
  PRIMARY KEY (account_id,ip)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='IPs recorded for any account'";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING IP TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the IP table exists
$request = "SELECT * FROM IP";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : IP table is missing.");

// NEWS
$existenceQuery = "SHOW TABLES LIKE 'News'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS News (
  id bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  game_id bigint(20) unsigned NOT NULL,
  date date NOT NULL,
  title varchar(255) NOT NULL DEFAULT 'News',
  text text,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='News of a game displayed when players log in' AUTO_INCREMENT=1";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING NEWS TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the News table exists
$request = "SELECT * FROM News";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : News table is missing.");

// REPORT
$existenceQuery = "SHOW TABLES LIKE 'Report'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Report (
  id bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  creation_date date NOT NULL,
  reporter_id bigint(20) unsigned NOT NULL,
  done_date date DEFAULT NULL,
  message text,
  screenshot longtext NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='All reports sent by players' AUTO_INCREMENT=1";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING REPORT TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Report table exists
$request = "SELECT * FROM Report";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Report table is missing.");


// SAVEGAME
$existenceQuery = "SHOW TABLES LIKE 'SaveGame'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS SaveGame (
  account_id bigint(20) unsigned NOT NULL,
  name varchar(255) NOT NULL,
  file longtext NOT NULL,
  PRIMARY KEY (account_id,name),
  UNIQUE KEY account_id (account_id,name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Example table on how to save files online'";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING SAVEGAME TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the SaveGame table exists
$request = "SELECT * FROM SaveGame";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : SaveGame table is missing.");


// CHATMESSAGE
$existenceQuery = "SHOW TABLES LIKE 'ChatMessage'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS ChatMessage (
  id bigint(20) NOT NULL AUTO_INCREMENT,
  server_id varchar(255) NOT NULL,
  account_id bigint(20) NOT NULL,
  message text,
  date datetime NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='All message from all servers' AUTO_INCREMENT=1";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING CHATMESSAGE TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the ChatMessage table exists
$request = "SELECT * FROM ChatMessage";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : ChatMessage table is missing.");


// FRIENDS
$existenceQuery = "SHOW TABLES LIKE 'Friends'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Friends (
  id_asker bigint(20) unsigned NOT NULL,
  id_asked bigint(20) unsigned NOT NULL,
  status varchar(255) NOT NULL DEFAULT 'Pending',
  PRIMARY KEY (id_asker,id_asked)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of friends and friends requests'";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING FRIENDS TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Friends table exists
$request = "SELECT * FROM Friends";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Friends table is missing.");


// JOINEDSERVER
$existenceQuery = "SHOW TABLES LIKE 'JoinedServer'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS JoinedServer (
  server_id varchar(255) NOT NULL,
  account_id bigint(20) NOT NULL,
  PRIMARY KEY (server_id,account_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING JOINEDSERVER TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the JoinedServer table exists
$request = "SELECT * FROM JoinedServer";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : JoinedServer table is missing.");


// SERVER
$existenceQuery = "SHOW TABLES LIKE 'Server'";
if(mysqli_num_rows(mysqli_query($databaseConnection, $existenceQuery))==0)
{
	$SQLrequest = "CREATE TABLE IF NOT EXISTS Server (
  id bigint(20) NOT NULL AUTO_INCREMENT,
  creator_id bigint(20) NOT NULL,
  name varchar(255) NOT NULL,
  capacity int(255) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='The table of all available servers' AUTO_INCREMENT=1";
	$SQLresult = mysqli_multi_query($databaseConnection, $SQLrequest) or die("ERROR DURING SERVER TABLE CREATION : ".mysqli_error($databaseConnection));
}
// Is the Server table exists
$request = "SELECT * FROM Server";
$results = mysqli_query($databaseConnection, $request) or die("ERROR : Server table is missing.");


// ------------------ Close connection and leave
mysqli_close($databaseConnection);
echo 'SUCCESS';

?>