The WebGL_BUILDS_Folder.zip is an optional install.

WULogin now includes a shortcode to simplify the displaying of WebGL content on your WordPress website.
If you intend to make use of this shortcode then you are going to need the contents of this zip file.

The easiest way to get started is to log into your website using an FTP client or using your web host's
file manager and extracting the folder into the root of your domain. The default settings should be sufficient
to get this working in that case. 

This means you will have a new folder called BUILDS in the root of your domain and inside this folder will be
two files: wuss_index.php and nogame.png
I recommend leaving it like this and uploading your files here but if you choose another folder, just make sure
to copy these two files to that folder. Note: it should work just fine but has not been extensively tested.

TLDR SHORT SHORT VERSION
========================

1. Extract this zip file into the root of your domain
2. After building your WebGL game, delete the index.html file and upload your game's build folder to www.yoursite.com/BUILDS/
3. Add the wollowing to your web page(s) [wuss_webgl game=mygame height=500 autoload=true]

Done. Just replace "mygame" above with the name of the folder you uploaded and you are good to go. 
For more reatures, read on...


PREPARING FOR USE
=================

When going to the WUSS main panel in the dashboard you will find a setting called "WebGL folder"
Enter the path to where you intend to upload your WebGL content to. I.e. http://www.mysite.com/BUILDS/
Remember to include the trailing backslash. If you do not enter anything into this field the shortcode will
assume you extracted the zip into the root of your domain and will use "/BUILDS/" as the URL.

Below it is a field called "WebGL placeholder graphic". This shortcode allows you to specify the size that
the WebGL content should take up on your web page but in order to speed up the loading of the page and to 
prevent people with download caps to leave your page because the page automatically loads the game, by default
it shows a placeholder graphic that the player must click on first in order for the WebGl content to load.

This field must contain the full path to a placeholder graphic you want to show before the player loads the WebGL content.
If you do not specify a graphic of your own it will default to the one included in the zip file:
	http://www.mysite.com/BUILDS/nogame.png

Alternatively, if the WebGL content belongs to a game you defined in WUSS, you can specify the game's Game ID
in the shortcode and it will then use the gamme's poster as the placeholder graphic. Example:
[wuss_webgl game=boulderdash gid=221]

Alternatively, you also have the option of NOT showing a placeholder graphic at all and just start the player immediately
by using the "autoload" parameter with a value of "true". 

When you build the player in Unity it will ask you for a folder to install into. Once building is complete you will have a
folder that contains your index.html file and a foler called Build. Simply delete everything in that folder except for the
"Build" folder and then upload the folder to the path you specified in "WebGL folder" in the dashboard.

Example, if you built the game to c:\builds\boulderDashV2 then you will have the following created for you:
c:\builds\boulderDashV2\index.html
c:\builds\boulderDashV2\Build (and your game will then be inside this)

If you specified http://www.mysite.com/BUILDS/ as your WebGL folder then you should upload the boulderDashV2 folder
to that location but, to reiterate, you ONLY need the Build folder, nothing else. This means you will end up with this:
http://www.mysite.com/BUILDS/boulderDashV2/Build
You will have the same path for every WebGL player you upload except for the name of the folder you originally built it into.

On my site I have 4 demos called UDEA0, UDEA1, UDEA2, and UDEA3 all neatly placed into:
http://www.mysite.com/BUILDS/UDEA0
http://www.mysite.com/BUILDS/UDEA1
http://www.mysite.com/BUILDS/UDEA2
http://www.mysite.com/BUILDS/UDEA3

Example use in my case:
     [wuss_webgl game=UDEA0 height=600]


EXAMPLES OF FULL IMPLEMENTATIONS
================================

[wuss_webgl game=boulderdash gid=221 width=90% height=300]
[wuss_webgl game=boulderdash autoload=true width=800 height=500]

Minimal use:
[wuss_webgl game=boulderdash height=500]

NOTES
=====

Do not use any suffix apart from % when specifying the size of the area
Do not use % for the height as that seems to always result in a height of 0
Default values are: height=100% (i.e. 0px), width=100% gid=-1 autoload=false
I do not currently make provision for the use of the Full Screen feature
