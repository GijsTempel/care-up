<?php
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Verify received data from the game
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////


include_once 'Includes/Functions.php';										// Include protection functions
include_once 'Includes/ServerSettings.php';									// Include server settings to get the configuration in order to connect to database
include_once 'Includes/InitServerConnection.php';							// Initialize the connection to the server

// Verify information are well received, in any other cases -> show error + stop Session script (connection to the database stopped properly)
if(!isset($_POST["GameName"])) { end_script("Error : DataVerifications.php - GameName not received."); }
if(!isset($_POST["GameVersion"])) { end_script("Error : DataVerifications.php - GameVersion not received."); }

// Get post information received from the game
$gameName = $_POST["GameName"]; 					// The game name received
$gameVersion = $_POST["GameVersion"]; 				// The game version received

?>