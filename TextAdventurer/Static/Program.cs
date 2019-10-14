using System;

/// <summary>
/// Text Adventurer namespace
/// </summary>
namespace TextAdventurer
{
    /// <summary>
    /// Program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Mein entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Story story = Story.LoadStory(args[0]);
                if (story == null)
                {
                    Console.Error.WriteLine("Could not open story \"" + story + "\"");
                }
                else
                {
                    story.Run();
                }
            }
        }
    }
}
