public class Song
{
    private const int MinLengthName = 3;
    private const int MaxLengthArtistName = 20;
    private const int MaxLengthSongName = 30;
    private const int MinimumLength = 0;
    private const int MaxMinutes = 14;
    private const int MaxSeconds = 59;

    private string artistName;
    private string songName;
    private int minutes;
    private int seconds;

    public Song(string artist, string name, int minutes, int seconds)
    {
        this.ArtistName = artist;
        this.SongName = name;
        this.Minutes = minutes;
        this.Seconds = seconds;
    }

    public string ArtistName
    {
        get
        {
            return this.artistName;
        }

        private set
        {
            if (string.IsNullOrWhiteSpace(value) ||         // razlika 2
                value.Length < MinLengthName ||
                value.Length > MaxLengthArtistName)
            {
                throw new InvalidArtistNameException();
            }

            this.artistName = value;
        }
    }

    public string SongName
    {
        get
        {
            return this.songName;
        }

        private set
        {
            if (string.IsNullOrWhiteSpace(value) ||     // razlika 1
                value.Length < MinLengthName ||
                value.Length > MaxLengthSongName)
            {
                throw new InvalidSongNameException();
            }

            this.songName = value;
        }
    }

    public int Minutes
    {
        get
        {
            return this.minutes;
        }

        private set
        {
            if (value < MinimumLength || value > MaxMinutes)
            {
                throw new InvalidSongMinutesException();
            }

            this.minutes = value;
        }
    }

    public int Seconds
    {
        get
        {
            return this.seconds;
        }

        private set
        {
            if (value < MinimumLength || value > MaxSeconds)
            {
                throw new InvalidSongSecondsException();
            }

            this.seconds = value;
        }
    }
}