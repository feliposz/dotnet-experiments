namespace TeleprompterConsole;

class Program
{
    static void Main(string[] args)
    {
        RunTeleprompter().Wait();
    }

    static async Task RunTeleprompter()
    {
        var config = new TelePrompterConfig();
        var displayTask = ShowTeleprompter(config);
        var speedTask = GetInput(config);
        await Task.WhenAny(displayTask, speedTask);
    }

    static async Task ShowTeleprompter(TelePrompterConfig config)
    {
        var words = ReadFrom("sampleQuotes.txt");
        foreach (var word in words)
        {
            Console.Write(word);
            if (!String.IsNullOrEmpty(word))
            {
                await Task.Delay(config.DelayInMilliseconds);
            }
        }
        config.SetDone();
    }

    static IEnumerable<string> ReadFrom(string file)
    {
        string? line;
        using (var reader = File.OpenText(file))
        {
            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split(' ');
                var lineLength = 0;
                foreach (var word in words)
                {
                    lineLength += word.Length;
                    if (lineLength > 70)
                    {
                        yield return Environment.NewLine;
                        lineLength = 0;
                    }
                    yield return word + " ";
                }
                yield return Environment.NewLine;
            }
        }
    }

    static async Task GetInput(TelePrompterConfig config)
    {
        Action work = () =>
        {
            do
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == '>')
                {
                    config.UpdateDelay(-10);
                }
                else if (key.KeyChar == '<')
                {
                    config.UpdateDelay(10);
                }
                else if (key.KeyChar == 'X' || key.KeyChar == 'x')
                {
                    config.SetDone();
                }
            } while (!config.Done);
        };
        await Task.Run(work);
    }
}


class TelePrompterConfig
{
    public int DelayInMilliseconds { get; private set; } = 200;
    public bool Done { get; private set; }

    public void UpdateDelay(int increment)
    {
        var newDelay = Math.Min(DelayInMilliseconds + increment, 1000);
        newDelay = Math.Max(newDelay, 20);
        DelayInMilliseconds = newDelay;
    }

    public void SetDone()
    {
        Done = true;
    }

}