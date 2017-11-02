using System;
using System.Collections.Generic;
using System.Linq;

public class Team
{
    private string name;
    private List<Player> players;

    public Team(string name)
    {
        this.Name = name;
        this.players = new List<Player>();
    }

    public string Name
    {
        get
        {
            return this.name;
        }

        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value == " ")
            {
                throw new ArgumentException("A name should not be empty.");
            }

            this.name = value;
        }
    }

    public int Rating
    {
        get
        {
            if (this.players.Count == 0)
            {
                return 0;
            }

            return this.GetRating();
        }
    }

    public void AddPlayer(Player playerToAdd)
    {
        this.players.Add(playerToAdd);
    }
                                                        
    public void RemovePlayer(string removeName)         
    {                                                   
        if (!this.players.Any(p => p.Name == removeName))
        {
            throw new ArgumentException($"Player {removeName} is not in {this.Name} team.");
        }

        var playerToRemove = this.players.Where(p => p.Name == removeName).ToList();

        this.players.Remove(playerToRemove[0]);
    }

    public override string ToString()
    {
        return $"{this.Name} - {this.Rating}";
    }

    private int GetRating()
    {
        double allSkills = 0;

        foreach (var player in this.players)
        {
            allSkills += player.Skill;
        }

        double rating = Math.Round(allSkills / this.players.Count, 0);

        return (int)rating;
    }
}
