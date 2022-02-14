using Terminal.Gui;

namespace RetroChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var top = Application.Top;

            var chatWindow = new ChatWindow("Retro Chat");
            top.Add(chatWindow);

            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_Application", new MenuItem[]
                {
                    new MenuItem("_Quit", "", () => Application.RequestStop())
                }),
                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "", () =>
                        MessageBox.Query(10, 5, "About", "Written by Felipo Soranz\nVersion 0.1", "Ok"))
                })
            });
            top.Add(menu);

            var loginWindow = new LoginWindow(null);
            // loginWindow.OnExit += () =>
            // {
            //     Application.RequestStop();
            // };
            loginWindow.OnLogin += (loginData) =>
            {
                chatWindow.Login(loginData.name);
                DummyChat.StartSimulation();
                Application.Run(top);
                DummyChat.StopSimulation();
            };

            DummyChat.OnUserAdded += (loginData) =>
            {
                // for thread-safety
                Application.MainLoop.Invoke(() =>
                {
                    chatWindow.AddUser(loginData.name);
                });
            };

            DummyChat.OnUserMessage += (message) =>
            {
                // for thread-safety
                Application.MainLoop.Invoke(() =>
                {
                    chatWindow.AddMessage(message.text, message.name);
                });
            };

            Application.Run(loginWindow);
        }
    }
}