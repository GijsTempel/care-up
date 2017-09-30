<?php


/**
 * The base configuration for WordPress
 *
 * The wp-config.php creation script uses this file during the
 * installation. You don't have to use the web site, you can
 * copy this file to "wp-config.php" and fill in the values.
 *
 * This file contains the following configurations:
 *
 * * MySQL settings
 * * Secret keys
 * * Database table prefix
 * * ABSPATH
 *
 * @link https://codex.wordpress.org/Editing_wp-config.php
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define('DB_NAME', 'triplemoti_TTMtheme');

/** MySQL database username */
define('DB_USER', 'triplemoti_TMM');

/** MySQL database password */
define('DB_PASSWORD', 'VjWblwKq5k');

/** MySQL hostname */
define('DB_HOST', 'localhost');

/** Database Charset to use in creating database tables. */
define('DB_CHARSET', 'utf8mb4');

/** The Database Collate type. Don't change this if in doubt. */
define('DB_COLLATE', '');

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         '|,$h w8!Q;(5Ij/`n45k?*;wMigd^2@{Gj6=>[F&F4FtpbOg*VBhTRKhp6VSy]+{');
define('SECURE_AUTH_KEY',  'Vb+W(-%5HS!3o{_ TCcb~.A>0OOZ3M>5hC$IbtGT)/GN<ush.BQSses>03*H/w:x');
define('LOGGED_IN_KEY',    'vbv9``#9p>&VN1F*/M*KQOxr!05`j&:QjJNS:}MPY$`q$6dW4^iijGTH(CUiAF/d');
define('NONCE_KEY',        'WjU2l8cr1*0ma^*IQd,D81~fjKL85zvk*W1o{y3%+m)Wbti(Zf.PFlI#ch!>*mK5');
define('AUTH_SALT',        '8%%DA+CF8}+?GQbW*gz*9/o#}{WPjIw~X!Kqw9~*7^t3_7jqvNz5;NXX^).]3WJ`');
define('SECURE_AUTH_SALT', 'kcT2DYY[:a]YOsP;~pUwl!(+RdU2(/Q&[n},w+@zB`)e|Bd1P<ly9h6 7TgyT@Je');
define('LOGGED_IN_SALT',   'LteHpXm!USU{3o-r8zo5)] givMEY N!KKjaR)H1Ch&m2[S[NbahAf|Y*e@[ps^x');
define('NONCE_SALT',       'CA$z%F:-M78@Q6KiFuBq^-9OD7WWCM:*su ^#22XP1l#d*cABJ9U|Cp,os1JOdM4');

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each
 * a unique prefix. Only numbers, letters, and underscores please!
 */
$table_prefix  = 'wp_';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 *
 * For information on other constants that can be used for debugging,
 * visit the Codex.
 *
 * @link https://codex.wordpress.org/Debugging_in_WordPress
 */
define('WP_DEBUG', false);

/* That's all, stop editing! Happy blogging. */

/** Absolute path to the WordPress directory. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');
