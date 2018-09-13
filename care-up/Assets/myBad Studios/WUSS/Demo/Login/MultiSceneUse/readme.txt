ABOUT THIS DEMO
===============

It has come to my attention that not everybody is able to figure out how to use this kit
when moving across scenes. If you destroy the prefab then you loose access to your ability
to contact the server but if you use DontDestroyOnLoad then you get duplicate prefabs each
time you go back to your menu scene.

For some this problem is something that is so obvious it doesn't even warrant a second thought
but for others it can take days of explaining and it still won't make any sense.

This demo was created with the sole purpose of demonstrating, in as easy as possible a manner,
how to overcome the multiple scene problem.

THE THEORY BEHIND IT ALL
========================

This entire demo only consists of 6 lines of code and that includes the ability to loop
between two scenes so explaining the code is going to be super easy. Instead, let's first 
discuss the theory behind what I am trying to do...

First of all, we need a copy of WPServer in the game at all times because that is how we
contact the server. Solution: Load WPServer
Secondly, we only want 1 instance on the prefab in the scene. Solution: Only load it once

And there we have it. The problem and the solution. So why is this seemingly simple task
such a big problem? Because people want to keep the entire prefab loaded so they use
DontDestroyOnLoad and then keep loading the same scene over and over. Obviously that is going 
to load duplicate prefabs because they keep loading the scene that already contains what they
already loaded before... So what now?

My solution to this problem is extremely simple. There are two ways to approach this
issue. Let me start with the more simple of the two...

HANDS ON: THE SOLUTION
======================

I only want WPServer loaded all the time, not the entire prefab. So I remove the
WPServer component from the prefab in the MainMenu scene and I allow that prefab to be
destroyed when the scene changes.

I now create another scene called 'Bootstrap' and I load this scene first. This scene 
contains all the objects that I want to load once only and then keep in memory so I
create an empty game object and attach such a component(s) to it. In this case I attach WPServer.cs
and I set it to not destroy. Now I can continue to load my audio manager, my inventory and
and other such objects that I want to load once and keep alive forever.

This scene now needs one final thing. it needs a script that immediately loads the next scene.
In this case the main menu.

So now we have a scene called Bootstrap that loads first, loads the WPServer singleton and then
immediately goes to the main menu. The main menu doesn't have WPServer on it and gets destroyed 
when the scene changes so now, all we need is the game scene and to make the menu load it. 

That is essentially it. We are done. But just to create a complete experience, let's make the 
game scene load the menu scene again when you press a button. Now you can see that you can load the
menu scene as many times as you want and there is no duplication involved and yet, WPServer is
always loaded and ready to be called. 

Problem solved!

THE CODE TO MAKE THIS ALL HAPPEN
================================

First, for the Bootstrap scene we create the GoToMenuScene class and give it only 1 line of code.
All we do is load the menu scene immediately upon loading this scene.

		public class GoToMenuScene : MonoBehaviour {
			void Start () => SceneManager.LoadScene("menu");	
		}

Next, for the menu scene we create the GoToGame class. This class will be used in two differnt ways.
The first is when login succeeds. At this point we want to automatically go directly to the game.
The second is when we return to this scene from the game. It will be displaying the PostLoginScene so
we want the "Resume Playing" button to take us back to the game again... 

We do this by registering to the onLoggedIn event in WULogin as well as the onResumeGame event that
is triggered by the "Resume Game" button. In both cases we call a function that loads the game scene.
This takes a total of 4 lines of code to accomplish.

		public class GoToGame : MonoBehaviour {
		    void Start()
		    {
				WULogin.onLoggedIn += GoToGameScene;
		        WULogin.onResumeGame += ResumeGame;
			}
		    void GoToGameScene( CML response ) => SceneManager.LoadScene("gamescene");
			void ResumeGame() => SceneManager.LoadScene("gamescene");
		}

Finally, in the game scene we create a button and we configure it to return to the menu scene when clicked.
for this we once again create a class that contains only 1 line of code, the goBackToMenu class:
		
		public class goBackToMenu : MonoBehaviour {
		    public void GoBack() => SceneManager.LoadScene( "menu" );
		}

And with that we are done.

THE ALTERNATIVE WAY: KEEP THE ENTIRE PREFAB IN MEMORY
=====================================================

If I have no need of the login prefab in my scene then the above method works just great. Alternatively
I usually load the prefab when I have a need for it and place it in a canvas so that I can destroy it manually
or have it automatically destroyed when the scene changes.

In some instances, though, you might prefer to actualyl have the entire prefab loaded all the time.
This is achieved easily enough by placing the login prefab that contains it's own canvas in the bootstap scene
instead of placing only WPServer inside of Bootstrap. Of course, make sure to enable dont_destroy_canvas on
WPServer so your prefab will remain alive throughout the game.

Now we run into the only real issue we have... Normally when you load the menu scene there is a menu prefab
ready and waiting to be displayed. Now we have the menu loaded all the time and most of the time it is disabled
so what happens when we enter the menu scene now? Nothing. Obviously that is a problem.

To overcome this we need to create a new script that looks for the prefab that is in memory and activates it.
Simple enough apart from one thing... The prefab normally checks to see if we are logged in or not and then
displays the relevant screen during Start(). Since the prefab is no longer doing that for us we need to do it
ourselves and decide if we want to see the Login menu or the PostLogin menu. 

So, first create an empty game object in the scene and then create the ShowMenu class to place on it:

public class ShowMenu : MonoBehaviour {
	void Start () {
        WUUGLoginGUI gui = FindObjectOfType<WUUGLoginGUI>();
        if ( WULogin.logged_in )
            gui?.ShowPostLoginMenu();
        else
            gui?.ShowLoginMenuScreen();
	}
}

And again... we are done.

Notice what that class does... During Start() it decides it wants to make use of the login prefab so
it does a search for it and once it has found it, it does whatever it is it feels it wants to do with that prefab.
You will now do the same in the game scene and any other scene in which you want to access the login prefab.
By loading it inside of the bootstrap scene you have made it available to all the scenes in your game and you
no longer need to return to the menu scene to see it.

IN CLOSING
==========

Now you have an understanding of two different ways in which you can use the login prefab across scenes.
Either by loading WPServer only or by loading the entire prefab into memory during the bootstrap scene that
is run as soon as the game starts and then never ever again!!!

If you choose to load the entire prefab then your menu scene will be empty and contain only a game object that
contains the ShowMenu component. 

And that is that... I don't think there is anything more that needs saying. Try out the demo scene for yourself.
If you want to try the alternate way then simply modify Bootstrap by deleting WPServer from there and adding
WULoginCanvas instead. Now edit the menu scene by deleting the login prefab and adding the ShowMenu component to
any of the existing game objects in the scene. Open the Bootstrap scene and hit play...

Enjoy...