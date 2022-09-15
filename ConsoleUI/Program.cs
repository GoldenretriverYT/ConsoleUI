using ConsoleUILib;
using ConsoleUILib.Window;

UIManager um = new();

um.Windows.Add(new EmptyWindow(2, 2, 20, 10));
um.Windows[0].Title = "amogus";

um.Windows.Add(new EmptyWindow(10, 10, 30, 30));
um.Windows[1].Title = "sus 2";

um.Start();