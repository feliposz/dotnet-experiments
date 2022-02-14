using Terminal.Gui;

namespace RetroChat
{
    public class LoginWindow : Window
    {
        private readonly View? _parent;
        public Action<(string? name, DateTime? birthday)>? OnLogin { get; set; }
        public Action? OnExit { get; set; }

        public LoginWindow(View? parent) : base("Login")
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }

        public void Close()
        {
            Application.RequestStop();
            _parent?.Remove(this);
        }

        private void InitStyle()
        {
            X = Pos.Center();
            Y = 5;
            Width = Dim.Percent(40);
            Height = 8;
        }

        private void InitControls()
        {
            var nameLabel = new Label(0, 0, "Nickname");
            var nameText = new TextField("")
            {
                X = Pos.Left(nameLabel),
                Y = Pos.Top(nameLabel) + 1,
                Width = Dim.Fill()
            };
            Add(nameLabel);
            Add(nameText);

            var birthLabel = new Label("Birthday")
            {
                X = Pos.Left(nameText),
                Y = Pos.Top(nameText) + 1
            };
            var birthText = new TextField("")
            {
                X = Pos.Left(birthLabel),
                Y = Pos.Top(birthLabel) + 1,
                Width = Dim.Fill()
            };
            Add(birthLabel);
            Add(birthText);

            var loginButton = new Button("Login", true)
            {
                X = Pos.Left(birthText),
                Y = Pos.Top(birthText) + 1
            };

            var exitButton = new Button("Exit")
            {
                X = Pos.Right(loginButton) + 5,
                Y = Pos.AnchorEnd(0)
            };

            Add(loginButton);
            Add(exitButton);

            loginButton.Clicked += () =>
            {
                if (nameText.Text.IsEmpty)
                {
                    MessageBox.ErrorQuery(25, 8, "Error", "Name cannot be empty.", "Ok");
                    return;
                }

                var isDateValid = DateTime.TryParse(birthText.Text.ToString(), out DateTime birthDate);

                if (birthText.Text.IsEmpty || !isDateValid)
                {
                    MessageBox.ErrorQuery(25, 8, "Error", "Date is required\nor is invalid.", "Ok");
                    return;
                }
                var name = nameText.Text.ToString();
                OnLogin?.Invoke((name, birthDate));

                Close();
            };

            exitButton.Clicked += () =>
            {
                OnExit?.Invoke();
                Close();
            };
            
            
        }
    }
}