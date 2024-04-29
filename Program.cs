using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace MOOOV
{
    public class Program
    {
        // Define file paths for each color
        private static Dictionary<string, string> colorAudioPaths = new Dictionary<string, string>()
        {
            {"black", @"C:\Programming\Semester 4\Week 2\GameCoding\bin\Debug\net8.0\black.mp3"},
            {"yellow", @"C:\Programming\Semester 4\Week 2\GameCoding\bin\Debug\net8.0\yellow.mp3"},
            {"red", @"C:\Programming\Semester 4\Week 2\GameCoding\bin\Debug\net8.0\red.mp3"},
            {"blue", @"C:\Programming\Semester 4\Week 2\GameCoding\bin\Debug\net8.0\blue.mp3"},
            {"green", @"C:\Programming\Semester 4\Week 2\GameCoding\bin\Debug\net8.0\green.mp3"},
            {"white", @"C:\Programming\Semester 4\Week 2\GameCoding\bin\Debug\net8.0\white.mp3"}
        };

        public static void Main()
        {
            Console.Clear();
            DisplayTitleScreen();

            int sequenceLength = 1;
            int startingSequenceDuration;
            int volatileSequenceDuration = 0;

            int difficulty = ChooseDifficulty();
            switch (difficulty)
            {
                case 1:
                    startingSequenceDuration = 4;
                    break;
                case 2:
                    startingSequenceDuration = 3;
                    break;
                case 3:
                    startingSequenceDuration = 2;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Defaulting to normal difficulty.");
                    startingSequenceDuration = 3;
                    break;
            }

            // Initialize the sequence outside the loop
            List<string> sequence = new List<string>();

            while (true)
            {
                volatileSequenceDuration += startingSequenceDuration;
                PlaySequence(sequence, sequenceLength, volatileSequenceDuration);
                sequenceLength++;

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void DisplayTitleScreen()
        {
            Console.WriteLine("Welcome to the Color Sequence Game!");
            Console.WriteLine("Press Enter to start...");
            Console.ReadLine();
            Console.Clear();
        }

        public static int ChooseDifficulty()
        {
            int choice = 0;
            while (true)
            {
                Console.WriteLine("Choose your difficulty:");
                Console.WriteLine("1. Easy");
                Console.WriteLine("2. Normal");
                Console.WriteLine("3. Hard");
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 3)
                    break;
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
            }
            return choice;
        }

        public static void PlaySequence(List<string> sequence, int length, int duration)
        {
            string[] colors = { "black", "yellow", "red", "blue", "green", "white" };
            Console.Clear();
            Console.WriteLine($"Get ready for round #{length}");

            // Add a new color to the sequence
            string randomColor;
            do
            {
                randomColor = FetchColor(colors, sequence);
            } while (sequence.Count > 0 && sequence[sequence.Count - 1] == randomColor);

            sequence.Add(randomColor);

            // Play the entire sequence
            foreach (string color in sequence)
            {
                Console.WriteLine(color);
                PlayAudio(color);
                Thread.Sleep(1000);
            }

            Console.WriteLine($"Starting countdown of {duration} seconds...");
            for (int i = duration; i > 0; i--)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }

            PlayBeep();

            Console.WriteLine("Time's up!");
            Console.ReadLine();
        }

        public static string FetchColor(string[] colors, List<string> sequence)
        {
            int index;
            string randomColor;

            do
            {
                index = new Random().Next(colors.Length);
                randomColor = colors[index];
            } while (sequence.Count > 0 && sequence[sequence.Count - 1] == randomColor);

            return randomColor;
        }

        public static void PlayAudio(string color)
        {
            if (colorAudioPaths.ContainsKey(color))
            {
                string filePath = colorAudioPaths[color];
                using (var audioFile = new AudioFileReader(filePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            else
            {
                Console.WriteLine($"Audio file for color {color} not found.");
            }
        }

        public static void PlayBeep()
        {
            Task.Run(() =>
            {
                Console.Beep();
            });
        }
    }
}
