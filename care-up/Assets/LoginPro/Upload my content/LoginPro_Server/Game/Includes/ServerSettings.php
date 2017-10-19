<?php
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Configure your server information
//
//	This file HAS TO BE protected !!
//	IF SOMEONE CAN READ THIS FILE : YOU PROTECTION IS DEAD !
//	.htaccess forbid all access to the entire folder so be caution not to place it elsewhere !
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Your domain
$_SESSION['Domain'] = 'www.triplemotionmedia.nl';
// The folder (or path) where you put the LoginAccount+ProSecure folder (initially : LoginAccount+ProSecure, if you didn't change it)
$_SESSION['SecureLoginFolder'] = 'LoginPro_Server/Game';

// Your host 
$_SESSION['SERVER_host'] = 'localhost'; // Caution : keep 'localhost' EXCEPT if your database server is not on your server (expert only)
// Your username to connect to the database
$_SESSION['SERVER_user'] = 'triplemoti_TMM';
// Your password to connect to the database 
$_SESSION['SERVER_password'] = 'VjWblwKq5k';
// The name of your database
$_SESSION['DB_name'] = 'triplemoti_TTMtheme';
// The table where your accounts are saved
$_SESSION['AccountTable'] = 'Account';
// The table where your achievements are saved
$_SESSION['AchievementsTable'] = 'Achievements';
// The table where your games are saved
$_SESSION['GameTable'] = 'Game';
// The table where players gaming sessions are saved
$_SESSION['GamingTable'] = 'Gaming';
// The table where your IPs are saved
$_SESSION['IPTable'] = 'IP';
// The table where game news are saved
$_SESSION['NewsTable'] = 'News';
// The table where your blocked IPs are saved
$_SESSION['AttemptsTable'] = 'Attempts';
// The table where the saveGame information example are saved
$_SESSION['SaveGame'] = 'SaveGame';
// The table used to report abuses
$_SESSION['Report'] = 'Report';
// The table where friend list and friend requests are saved
$_SESSION['Friends'] = 'Friends';
// The table of the servers
$_SESSION['Server'] = 'Server';
// The table of the chat message
$_SESSION['ChatMessage'] = 'ChatMessage';

// Your contact email (in case you want to send email validations), players will receive email from this email address (you could create a contact email address for example)
$_SESSION['SERVER_email'] = 'Info@careup.nl';
$_SESSION['SERVER_emailPassword'] = 'Care Up Support team';

// The maximum number of wrong attempts before IP being blocked for an account
$_SESSION['AvailableAttemptsBeforeBlocking'] = 10;

// Scan clients IP
define('SCAN_IP_ACTIVATED', TRUE, TRUE);

?>