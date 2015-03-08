//-----------------------------------------------------------------------------
// MarbleSkinSelectionDlg.cs
//
// Copyright (c) 2015 Jeff Hutchinson
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function MarbleSkinSelectionDlg::onWake(%this) {
   MSSD_Text.setText("<font:DomCasualD:32><color:000000><just:center>Select Marble Skin!");   
   
   // build skin list
   MSSD_List.clear();
   %path = $Con::root @ "/data/shapes/balls/*.marble.*";
   %file = findFirstFile(%path);
   while (%file !$= "") {
      // 2 passes of filebase to strip skin name:
      //
      // 1. base.marble
      // 2. base
      %skin = fileBase(fileBase(%file));
      MSSD_List.addRow(MSSD_List.rowCount(), %skin);
      
      // search for next file
      %file = findNextFile(%path);
   }
   
   %count = MSSD_List.rowCount();
   if (%count == 0) {
      error("No marble skins found!");
      MessageBoxOK("Error", "You don't have any skins!", "Canvas.popDialog(MarbleSkinSelectionDlg);");
   } else {
      echo("Found" SPC %count SPC "marble skins!");
      
      %setId = 0;
      
      // set to current skin if we already have it
      %skin = trim($pref::marbleSkin);
      if (%skin !$= "") {
         for (%i = 0; %i < %count; %i ++) {
            %text = MSSD_List.getRowTextById(%i);
            if (%text $= %skin) {
               %setId = %i;
               break;
            }
         }
      }
      MSSD_List.setSelectedById(%setId);
      
      // set skin
      %skin = MSSD_List.getRowTextById(%setId);
      MSSD_ObjectView.setModel($Con::root @ "/data/shapes/balls/ball-superball.dts", %skin);      
      
      if (%count == 1) {
         // if theres only one, we only have one choice!
         // so disable prev/next buttons
         MSSD_prev.setActive(false);
         MSSD_next.setActive(false);
      }
   }
}

function MarbleSkinSelectionDlg::apply(%this) {
   $pref::marbleSkin = MSSD_List.getRowTextById(MSSD_List.getSelectedId());
   Canvas.popDialog(%this);
}

function MarbleSkinSelectionDlg::next(%this) {
   %id = MSSD_List.getSelectedId() + 1;
   if (%id >= MSSD_List.rowCount())
      %id = 0;
   MSSD_List.setSelectedById(%id);
   
   // set skin
   %skin = MSSD_List.getRowTextById(%id);
   MSSD_ObjectView.setModel($Con::root @ "/data/shapes/balls/ball-superball.dts", %skin);   
}

function MarbleSkinSelectionDlg::prev(%this) {
   %id = MSSD_List.getSelectedId() - 1;
   if (%id < 0)
      %id = MSSD_List.rowCount() - 1;
   MSSD_List.setSelectedById(%id);
   
   // set skin
   %skin = MSSD_List.getRowTextById(%id);
   MSSD_ObjectView.setModel($Con::root @ "/data/shapes/balls/ball-superball.dts", %skin);      
}

//-----------------------------------------------------------------------------

package ActivateMarbleSkins {   
   // applying the skin
   function GameConnection::createPlayer(%this, %spawnPoint) {
      Parent::createPlayer(%this, %spawnPoint);
      
      // set the marble skin after the player has been created.
      // If a skin was never chosen, assume the default skin.
      if (trim($pref::marbleSkin) $= "")
         %this.player.setSkinName("base");
      else
         %this.player.setSkinName($pref::marbleSkin);
   }
   
   // places the marble selection button on the level select screen.
   function playMissionGui::onWake(%this) {
      Parent::onWake(%this);
      
      // show button on playmission gui
      if (!isObject(PMG_MarbleSelect)) {
         PM_Box.add(new GuiButtonCtrl(PMG_MarbleSelect) {
            profile = "GuiButtonProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "200 260";
            extent = "100 61";
            text = "Marble Select";
            command = "Canvas.pushDialog(MarbleSkinSelectionDlg);";
         });
      }
   }   
};

// activate first package as soon as the script is executed
activatePackage(ActivateMarbleSkins);

// execute the GUI file after all the guis have been initialized
exec($Con::Root @ "/client/ui/MarbleSkinSelectionDlg.gui");