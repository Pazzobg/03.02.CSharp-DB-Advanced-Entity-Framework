namespace Stations.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Stations.Data;
    using Stations.Models;
    using Stations.Models.Enums;
    using Stations.DataProcessor.Dto.Export;

    public class Serializer
    {
        public static string ExportDelayedTrains(StationsDbContext context, string dateAsString)
        {
            var date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var result = new List<Train>();

            var delayedTrains = context.Trains
                .Where(t => t.Trips.Any(tr => tr.Status == TripStatus.Delayed && tr.DepartureTime <= date))
                .Select(t => new
                {
                    t.TrainNumber, 
                    DelayedTrips = t.Trips
                        .Where(tr => tr.Status == TripStatus.Delayed && tr.DepartureTime <= date)
                        .ToArray()
                })
                .Select(t => new TrainDto
                {
                    TrainNumber = t.TrainNumber,
                    DelayedTimes = t.DelayedTrips.Count(),
                    MaxDelayedTime = t.DelayedTrips.Max(d => d.TimeDifference).ToString()
                })
                .OrderByDescending(t => t.DelayedTimes)
                .ThenByDescending(t => t.MaxDelayedTime)
                .ThenByDescending(t => t.TrainNumber)
                .ToArray();

            var jsonString = JsonConvert.SerializeObject(delayedTrains, Newtonsoft.Json.Formatting.Indented);
            return jsonString;
        }

        public static string ExportCardsTicket(StationsDbContext context, string cardType)
        {
            var type = Enum.Parse<CardType>(cardType);
            var cards = context.Cards
                .Where(c => c.Type == type && c.BoughtTickets.Count > 0)
                .OrderBy(c => c.Name);

            var doc = new XDocument(new XElement("Cards"));

            foreach (var c in cards)
            {
                var currentCard = new XElement("Card", 
                                        new XAttribute("name", c.Name), 
                                        new XAttribute("type", cardType));

                var tickets = new XElement("Tickets");
                foreach (var t in c.BoughtTickets)
                {
                    tickets.Add(new XElement("Ticket",
                                        new XElement("OriginStation", t.Trip.OriginStation.Name),
                                        new XElement("DestinationStation", t.Trip.DestinationStation.Name),
                                        new XElement("DepartureTime", t.Trip.DepartureTime
                                                                       .ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture))));
                }

                currentCard.Add(tickets);
                doc.Root.Add(currentCard);
            }

            var result = doc.ToString();
            return result;

            /* Vladi's solution: 
            var type = Enum.Parse<CardType>(cardType);
            var cards = context.Cards
                .Where(c => c.Type == type && c.BoughtTickets.Any())
                .Select(c => new CardDto
                {
                    Name = c.Name,
                    Type = c.Type.ToString(),
                    Tickets = c.BoughtTickets.Select(t => new TicketDto
                    {
                        OriginStation = t.Trip.OriginStation.Name,
                        DestinationStation = t.Trip.DestinationStation.Name,
                        DepartureTime = t.Trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                    })
                    .ToArray()
                })
                .OrderBy(c => c.Name)
                .ToArray();

            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
            serializer.Serialize(new StringWriter(sb), cards, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty}));

            string result = sb.ToString();
            return result;*/
        }
    }
}