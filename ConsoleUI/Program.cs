using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;

CustomWindow amogus = new CustomWindow(2, 0, 20, 30);
amogus.Title = "Text Align Test";
amogus.RenderDone += (object sender, EventArgs e) => {
    amogus.Title = "Render at " + DateTimeOffset.Now.Hour.ToString("00") + ":" + DateTimeOffset.Now.Minute.ToString("00") + ":" + DateTimeOffset.Now.Second.ToString("00");
};

Button leftButton = new Button(amogus, 5, 2, 10, 2);
leftButton.Text = "dfsdfdfdsfsdfdf";
leftButton.HorizontalAlign = HAlign.LEFT;
leftButton.PressAnimation = ButtonPressAnimation.POP_OUT;
leftButton.Pressed += (object sender, EventArgs e) => {
    (sender as Button).Text = "Pressed!";
};

amogus.AddControl(leftButton);

Button centerButton = new Button(amogus, 5, 5, 10, 2);
centerButton.Text = "center";
centerButton.HorizontalAlign = HAlign.MIDDLE;
centerButton.Pressed += (object sender, EventArgs e) => {
    (sender as Button).Text = "Pressed!";
};

amogus.AddControl(centerButton);

Button rightButton = new Button(amogus, 5, 8, 10, 2);
rightButton.Text = "right";
rightButton.HorizontalAlign = HAlign.RIGHT;
rightButton.Pressed += (object sender, EventArgs e) => {
    (sender as Button).Text = "Pressed!";
};

amogus.AddControl(rightButton);

UIManager.AddWindow(amogus);
UIManager.Start();