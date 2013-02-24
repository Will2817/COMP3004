COMP3004
========

COMP3004 group assignments and projects

Setup:

Copy the 7Wonders folder to a temporary location and then delete the orginal.
Start Visual studio.
File->New->Project

Select Windows Game(4.0), if you don't see it look under:
Other Languages-> Visual C#->XNA Game Studio 4.0

Enter name 7Wonders and set the location to C:\Users\<yourname>\GitHub\COMP3004\
press ok

in the Solution Explorer right-click on 7WondersContent(Content) and add the following forlders
Fonts
Images
Images->Cards
Images->Wonders

copy 7Wonder folder back from temporary location and overwrite everything.

in the Solution Explorer right-click on 7Wonders-> add -> existing item
and select all .cs files.

in the Solution Explorer right-click -> add -> existing item on each folder
you made earlier and add all the files in each folder.

Update 2/24/2013:

there is now a Json folder to hold json files in the project.
add the folder and all files into your solution.

also you will have to go into the properties of each json file and set
the Build Action:None, and set Copy to Output Directory:"Copy if newer"

There is also now a Newtonsoft.Json dll in the program, to add it go to the
references folder (NOT IN THE CONTENT AREA... LOOK ABOVE) and add a reference to
it... you will have to you the browse to find it.





