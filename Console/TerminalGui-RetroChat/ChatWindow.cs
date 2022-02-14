using Terminal.Gui;

namespace RetroChat
{
    public class ChatWindow : Window
    {
        private string? _username;
        private List<string?> _messages = new List<string?>();
        private List<string?> _users = new List<string?>();
        private ListView _chatView = new ListView();
        private ListView _userList = new ListView();

        public ChatWindow(string title) : base(title)
        {
            InitStyle();
            InitControls();
        }

        public void Login(string? username)
        {
            _username = username;
            AddUser(username);
        }

        public void AddUser(string? username)
        {
            AddMessage($"User {username} has joined the chat at {DateTime.Now.ToLongTimeString()}.");
            _users.Add(username);
            _userList.SetSource(_users);
        }

        public void AddMessage(string? text, string? name = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                _messages.Add(text);
            }
            else
            {
                _messages.Add($"{name}: {text}");
            }
            _chatView.SetSource(_messages); // NOTE: set source forces redraw
            // auto-scroll
            _chatView.GetCurrentHeight(out int height);
            if (_messages.Count > height)
            {
                _chatView.ScrollDown(_messages.Count - height);
            }
        }

        private void InitStyle()
        {
            X = 0;
            Y = 0;
            Width = Dim.Fill();
            Height = Dim.Fill();
        }

        private void InitControls()
        {
            var chatViewFrame = new FrameView("Chats")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(75),
                Height = Dim.Percent(80),
            };
            _chatView = new ListView
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            chatViewFrame.Add(_chatView);
            Add(chatViewFrame);

            var userListFrame = new FrameView("Online Users")
            {
                X = Pos.Right(chatViewFrame),
                Y = Pos.Top(chatViewFrame),
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            _userList = new ListView
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            userListFrame.Add(_userList);
            Add(userListFrame);

            var chatBar = new FrameView(null)
            {
                X = 0,
                Y = Pos.Bottom(chatViewFrame),
                Width = chatViewFrame.Width,
                Height = Dim.Fill()
            };
            var chatMessage = new TextField("")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(75),
                Height = Dim.Fill()
            };
            var sendButton = new Button("Send", true)
            {
                X = Pos.Right(chatMessage),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            sendButton.Clicked += () =>
            {
                // for thread-safety
                Application.MainLoop.Invoke(() =>
                {
                    AddMessage(chatMessage.Text.ToString(), _username);
                    chatMessage.Text = "";
                });
            };
            chatBar.Add(chatMessage);
            chatBar.Add(sendButton);
            Add(chatBar);
        }

    }
}