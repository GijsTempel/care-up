myBad Studios Core
==================

This kit contains the classes I tend to use in every project I do.

They form the core of all my work and is thus made available as a separate download to facilitate
an easier upgrade process as they are improved upon. In time other parts of code may join the
ranks of the core group and will be added to this package when they do.

The core components include:
1. mbsStateMachine
------------------
My custom designed state machine. It allows me to create a layer of abstraction to replace long
and unsightly if-else code blocks. Super simple to use, no matter what project I'm doing I always
find a need for this...

2. mbsStateMachineLeech
-----------------------
Attach multiple state machines to the same state. Change the state on one machine and all other
states will perform their relevant action based on the new state of the source machine.

3. mbsSlider
------------
In video games, animations is always good. Static panels and stuff that just appear and disappear
in the blink of an eye just look amateurish. This is a low level class that you simply tell where
the final panel is to appear, what direction to slide in from and slide out to and then call Update
to do it. This class will then provide you with a Rect that you use to draw all your content to and
also throws out events when reaching the opened or closed state in case you want it.
Also contains code to fade the content in and out as the panel slides.

4. CML
------
CML is a custom data storage and retrieval system. To best describe it, think of it is as XML, only easier.
CML also contains a built in save and load system that utilises PlayerPrefs, Resources folders and file paths.

And finally, CML also allows you to generate variables dynamically at runtime and use them as typed variables.
CML is even smart enough to provide default values for when you use a variable that doesn't exist thereby
preventing runtime errors and allowing you to save less data to disc.

5. MBSSingleton
-------
If you are like me and make use of singletons a lot then you probably find yourself very frustrated with having
to code the exact same code for every singleton you have to create. No more. Simply change your class definition
to subclass from MBSSingleton instead of MonoBehaviour and you are done. All setup and ready to go.

6. PleaseWait
-------------
With a sigle line of code, display a spinning image in the center of the screen as a progress indicator.
You can add the PleaseWait component to a prefab and assign an image of your choice or you can just start
using it and it will create a persistent game object automatically. If it is created automatically then it
will require an image called Spinner inside a Resources foler.

7. Encoder
----------
Encode a string to MD5 or base64 and decode base64 back to string using a single line of code for each
operation, respectively

8. StatusMessage
----------------
Show error messages on screen. Messages can stack and will automatically disappear in a few seconds

9. mbsEventHandler & mbsEvent 
-----------------------------
mbsEventHandler is a predefined event handler that takes both a generic object and a CML as parameters that
it passes to your functions. As such this single event handler is the only one you are likely to need for
all your current and future projects

10. MBSEvents & MBSNotices
--------------------------
Instead of keeping track of dozens of events by name, have them auto initialized, null checked and cleaned up
for you. Reference them by enum value rather than by name meaning that adding a new event to your project,
having it null checked before use and cleared up now only requires that you change the enum decleration and
everything else is automated from there on out.

11. mbsNotification
-------------------
More and more I find myself wanting to show users a quick notification and then remove it from the screen.
In contrast to StatusMessage, mbsNotification allows you to show an optional graphic with your message,
show both a header and text, animate the notification in and out rather than just showing and disappearing 
and, finally, it also supports queues so you can safely trigger all the notifications you want without
worrying about whether or not there are any notifications showing already. Also, in contrast to StatusMessage
mbsNotification uses the Unity UI instead of the old OnGUI system.

12. MBS Utilities
-----------------
I extend various closed classes to provide new functionality or just to simplify using them.
Examples inlude converting Color to Hex for use with TextMeshPro or to test if an email string is properly formatted
Also allows you to update Vector3 and Vector2 values without first having to create a duplicate thereof.
Example: transform.position = transform.position.SetX(5f);

NOTE: ALL CLASSES ARE INSIDE THE MBS NAMESPACE.
MAKE SURE TO ADD "using MBS;" AT THE TOP

Included in the kit is a folder with some example scripts that demonstrate the use of each component.
Simply open up the source and follow along with the comments. To see the code in action, simply drop onto
an empty game object and hit play.