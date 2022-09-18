using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;

UIManager um = new();

CustomWindow amogus = new CustomWindow(2, 2, 20, 15);
amogus.Title = "Text Align Test";

Button leftButton = new Button(amogus, 5, 2, 10, 2);
leftButton.Text = "left";
leftButton.HorizontalAlign = HAlign.LEFT;

amogus.AddControl(leftButton);

Button centerButton = new Button(amogus, 5, 5, 10, 2);
centerButton.Text = "center";
centerButton.HorizontalAlign = HAlign.MIDDLE;

amogus.AddControl(centerButton);

Button rightButton = new Button(amogus, 5, 8, 10, 2);
rightButton.Text = "right";
rightButton.HorizontalAlign = HAlign.RIGHT;

amogus.AddControl(rightButton);

um.AddWindow(amogus);
um.Start();