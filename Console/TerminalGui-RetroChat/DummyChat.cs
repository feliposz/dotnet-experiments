namespace RetroChat
{
    public static class DummyChat
    {
        public static Action<(string name, DateTime birthday)>? OnUserAdded;
        public static Action<(string name, string text)>? OnUserMessage;

        private static readonly object _mutex = new object();
        private static Thread? _main;
        private static Random random = new Random();

        public static void StartSimulation()
        {
            lock (_mutex)
            {
                if (_main == null)
                {
                    _main = new Thread(new ThreadStart(Simulate));
                    _main.Start();
                }
            }
        }

        public static void StopSimulation()
        {
            _main?.Interrupt();
        }

        public static void Simulate()
        {
            try
            {
                const int maxUsers = 10;
                int counter = 0;
                while (++counter <= maxUsers)
                {
                    Thread.Sleep(1000);
                    var name = $"User {counter}";
                    OnUserAdded?.Invoke((name, DateTime.Now));
                }

                string[] messages = { "hello", "hi", "howdy", "hi there", "is anybody here?", "how are you?", "what's up?", ":)", "=]", ":-D", "8-)" };
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(2000);
                    var name = $"User {random.Next(maxUsers) + 1}";
                    var message = messages[random.Next(messages.Length)];
                    OnUserMessage?.Invoke((name, message));
                }
            }
            catch (ThreadInterruptedException)
            {
                // Stop simulation
            }
        }
    }
}