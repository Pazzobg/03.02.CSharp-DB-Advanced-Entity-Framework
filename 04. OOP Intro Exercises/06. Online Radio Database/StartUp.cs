using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var playlist = new List<Song>();

        int loopEnd = int.Parse(Console.ReadLine());

        for (int i = 0; i < loopEnd; i++)
        {
            string[] input = Console.ReadLine().Split(';');

            try
            {

                string artist = input[0];
                string songName = input[1];
                string[] songLength = input[2].Split(':');
                int minutes = int.Parse(songLength[0]);
                int seconds = int.Parse(songLength[1]);

                var song = new Song(artist, songName, minutes, seconds);

                playlist.Add(song);

                Console.WriteLine("Song added.");
            }
            catch (InvalidSongException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(FormatException)
            {
                Console.WriteLine(new InvalidSongLengthException().Message);
            }
        }

        var totalTime = playlist.Sum(t => t.Minutes * 60) + playlist.Sum(s => s.Seconds);
        long listSeconds = totalTime % 60;
        long listMinutes = (totalTime / 60) % 60;
        long listHours = (totalTime / 60 / 60) % 24;

        Console.WriteLine($"Songs added: {playlist.Count}");
        Console.WriteLine($"Playlist length: {listHours}h {listMinutes}m {listSeconds}s");
    }
}