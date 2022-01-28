using PluginBase;
using System.Reflection;

namespace AppWithPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "/d")
            {
                Console.WriteLine("Press <Enter> to continue...");
                Console.ReadLine();
            }

            string[] pluginPaths = new string[]
            {
                @"HelloPlugin\bin\Debug\net6.0\HelloPlugin.dll"
            };

            IEnumerable<ICommand> commands = pluginPaths.SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateCommands(pluginAssembly);
            }).ToList();

            if (args.Length == 0)
            {
                Console.WriteLine("Commands: ");
                foreach (ICommand command in commands)
                {
                    Console.WriteLine($"{command.Name}\t - {command.Description}");
                }
            }
            else
            {
                foreach (var commandName in args)
                {
                    Console.WriteLine($"-- {commandName} --");
                    var command = commands.FirstOrDefault(c => c.Name == commandName);
                    if (command == null)
                    {
                        Console.WriteLine("No such command is known.");
                        return;
                    }
                    command.Execute();
                    Console.WriteLine();
                }
            }
        }

        static Assembly LoadPlugin(string relativePath)
        {
            var GetDir = (string path) => Path.GetDirectoryName(path);

            string root = Path.GetFullPath(Path.Combine(
                GetDir(GetDir(GetDir(GetDir(GetDir(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<ICommand> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                {
                    var result = Activator.CreateInstance(type) as ICommand;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}