using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistConsoleApp
{
    // App to perform operations on playlist
    public class App
    {
        private static PlayList playList = null;
        private static Dictionary<string, int> commandMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Create", 1 },
            { "Insert", 2 },
            { "Delete", 3 },
            { "Shuffle", 4},
            { "Play", 5}
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Supported operations are 'Create', 'Delete', 'Insert', 'Shuffle', 'Play'");
            var shouldContinue = true;
            while (shouldContinue)
            {
                Console.WriteLine("Enter the operation to perform :");
                var operation = Console.ReadLine();
                var arguments = operation.Split(' ').Where(x => !x.Equals(""));
                try
                {
                    ExecuteCommand(arguments.ToArray());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    shouldContinue = false;
                    Console.WriteLine("Do you wish to retry ?? If yes, enter 1");
                    var ans = Console.ReadLine();
                    if(int.Parse(ans) == 1)
                    {
                        shouldContinue = true;
                    }
                }
            }
        }

        private static void ExecuteCommand(string[] args)
        {
            var command = CommandParser(args[0]);
            if (command > 1 && playList == null)
            {
                throw new InvalidOperationException("Playlist does not exists. Perform a 'Create' operation first");
            }

            switch (command)
            {
                case 1:
                    var size = int.Parse(args[1]);
                    playList = PlayList.Create(size);
                    break;

                case 2:
                    var ordinal = int.Parse(args[1]);
                    var value = int.Parse(args[2]);
                    playList.Insert(ordinal, value);
                    break;

                case 3:
                    ordinal = int.Parse(args[1]);
                    playList.Delete(ordinal);
                    break;

                case 4:
                    playList.Shuffle();
                    break;

                case 5:
                    ordinal = int.Parse(args[1]);
                    playList.Play(ordinal);
                    break;

                default:
                    throw new InvalidOperationException("Supported operations are 'Create', 'Delete', 'Insert', 'Shuffle', 'Play'");

            }

            playList.PrintPlaylist();
            Console.WriteLine();
        }

        private static int CommandParser(string command)
        {
            int commandValue = 0;
            commandMap.TryGetValue(command, out commandValue);
            return commandValue;
        }
    }
}
