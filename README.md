# MarbleSkinSelection
A marble skin selection mod for Marble Blast Gold! This will allow you to place custom marble and change your skins inside of marble blast gold. 

Simply put your skins inside of the folder located at "marble/data/shapes/balls/". In order for the skins to show up in the selector, name them skinname.marble.jpeg or skinname.marble.png!

![](http://i.imgur.com/B7CcfvF.png)

The skin that you choose will be saved inside of the prefs.cs file.

## Installation
1. Place the MarbleSkinSelectionDlg.gui file inside of marble/client/ui/ 

2. Place the MarbleSkinSelectionDlg.cs file inside of the marble/client/scripts/ folder. 

3. Inside of the root folder (where the executable is for the game), open up main.cs with a text editor, and at the bottom, add this line of code:

    exec($Con::root @ "/client/scripts/MarbleSkinSelectionDlg.cs");
	 
## License
This has been released under the MIT license.
