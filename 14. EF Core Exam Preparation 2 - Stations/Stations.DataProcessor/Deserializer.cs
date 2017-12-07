namespace Stations.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using Stations.Data;
    using Stations.DataProcessor.Dto.Import;
    using Stations.Models;
    using AutoMapper;
    using Stations.Models.Enums;
    using System.Globalization;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using Microsoft.EntityFrameworkCore;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            var stations = JsonConvert.DeserializeObject<Station[]>(jsonString);

            var validStations = new List<Station>();
            var sb = new StringBuilder();

            foreach (var station in stations)
            {
                if (station.Name == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (station.Town == null)
                {
                    station.Town = station.Name;
                }

                if (validStations.Any(s => s.Name == station.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (station.Name.Length > 50 || station.Town.Length > 50)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                validStations.Add(station);
                sb.AppendLine(string.Format(SuccessMessage, station.Name));
            }

            context.Stations.AddRange(validStations);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            var classes = JsonConvert.DeserializeObject<SeatingClass[]>(jsonString);

            var validClasses = new List<SeatingClass>();
            var sb = new StringBuilder();

            foreach (var seatClass in classes)
            {
                if (seatClass.Name == null || seatClass.Abbreviation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (seatClass.Name.Length > 30 || seatClass.Abbreviation.Length != 2)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (validClasses.Any(c => c.Name == seatClass.Name || c.Abbreviation == seatClass.Abbreviation))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                validClasses.Add(seatClass);
                sb.AppendLine(string.Format(SuccessMessage, seatClass.Name));
            }

            context.SeatingClasses.AddRange(validClasses);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            var trains = JsonConvert.DeserializeObject<TrainDto[]>(jsonString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var validTrains = new List<Train>();
            var sb = new StringBuilder();

            foreach (var trainDto in trains)
            {
                if (!IsValid(trainDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (validTrains.Any(t => t.TrainNumber == trainDto.TrainNumber))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatsValid = trainDto.Seats.All(IsValid);
                if (!seatsValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var properSeatingClasses = trainDto.Seats
                    .All(s => context.SeatingClasses
                                .Any(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation));
                if (!properSeatingClasses)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var trainType = Enum.Parse<TrainType>(trainDto.Type);

                var trainSeats = trainDto.Seats
                    .Select(s => new TrainSeat
                    {
                        SeatingClass = context.SeatingClasses
                                        .FirstOrDefault(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation),
                        Quantity = s.Quantity.Value
                    })
                    .ToArray();

                var train = new Train
                {
                    TrainNumber = trainDto.TrainNumber,
                    Type = trainType,
                    TrainSeats = trainSeats
                };

                validTrains.Add(train);
                sb.AppendLine(string.Format(SuccessMessage, trainDto.TrainNumber));
            }

            context.Trains.AddRange(validTrains);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            var trips = JsonConvert.DeserializeObject<TripDto[]>(jsonString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var validTrips = new List<Trip>();
            var sb = new StringBuilder();

            foreach (var tripDto in trips)
            {
                if (!IsValid(tripDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var train = context.Trains.FirstOrDefault(t => t.TrainNumber == tripDto.Train);
                var originStation = context.Stations.FirstOrDefault(s => s.Name == tripDto.OriginStation);
                var destStation = context.Stations.FirstOrDefault(s => s.Name == tripDto.DestinationStation);

                if (train == null || originStation == null || destStation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var departureTime = DateTime.ParseExact(tripDto.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var arrivalTime = DateTime.ParseExact(tripDto.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var status = Enum.Parse<TripStatus>(tripDto.Status);

                TimeSpan timeDifference;
                if (tripDto.TimeDifference != null)
                {
                    timeDifference = TimeSpan.ParseExact(tripDto.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture);
                }

                if (arrivalTime < departureTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var trip = new Trip
                {
                    Train = train,
                    OriginStation = originStation,
                    DestinationStation = destStation,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    TimeDifference = timeDifference,
                    Status = status
                };

                validTrips.Add(trip);
                sb.AppendLine($"Trip from {tripDto.OriginStation} to {tripDto.DestinationStation} imported.");
            }

            context.Trips.AddRange(validTrips);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            var customerCardsXml = XDocument.Parse(xmlString);
            var root = customerCardsXml.Root.Elements();

            var validCards = new List<CustomerCard>();
            var sb = new StringBuilder();

            foreach (var cardXml in root)
            {
                string name = cardXml.Element("Name").Value;
                int age = int.Parse(cardXml.Element("Age").Value);
                string type = cardXml.Element("CardType")?.Value ?? "Normal";

                var cardType = Enum.Parse<CardType>(type);

                if (name.Length > 128 || age < 0 || age > 120)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var card = new CustomerCard
                {
                    Name = name,
                    Age = age,
                    Type = cardType
                };

                validCards.Add(card);
                sb.AppendLine(string.Format(SuccessMessage, name));
            }

            context.Cards.AddRange(validCards);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;

            /* Vladi's solution: 
            var serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
            var deserializedCards = (CardDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();
            var validCards = new List<CustomerCard>();

            foreach (var cardDto in deserializedCards)
            {
                if (!IsValid(cardDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var cardType = Enum.TryParse<CardType>(cardDto.CardType, out var card) ? card : CardType.Normal;

                var customerCard = new CustomerCard
                {
                    Name = cardDto.Name,
                    Age = cardDto.Age,
                    Type = cardType
                };

                validCards.Add(customerCard);
                sb.AppendLine(string.Format(SuccessMessage, cardDto.Name));
            }

            context.Cards.AddRange(validCards);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;*/
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            var deserializedTickets = (TicketDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var validTickets = new List<Ticket>();
            var sb = new StringBuilder();

            foreach (var ticketDto in deserializedTickets)
            {
                if (!IsValid(ticketDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var departureTime = DateTime.ParseExact(ticketDto.Trip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var trip = context.Trips
                    .Include(t => t.OriginStation)
                    .Include(t => t.DestinationStation)
                    .Include(t => t.Train)
                    .ThenInclude(tr => tr.TrainSeats)
                    .SingleOrDefault(t => t.DepartureTime == departureTime &&
                                          t.OriginStation.Name == ticketDto.Trip.OriginStation &&
                                          t.DestinationStation.Name == ticketDto.Trip.DestinationStation);

                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;
                if (ticketDto.Card != null)
                {
                    card = context.Cards.SingleOrDefault(c => c.Name == ticketDto.Card.Name);

                    if (card == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                var seatingClass = ticketDto.Seat.Substring(0, 2);
                var seatNumber = int.Parse(ticketDto.Seat.Substring(2));

                var seatExists = trip.Train.TrainSeats
                    .SingleOrDefault(s => s.SeatingClass.Abbreviation == seatingClass && s.Quantity >= seatNumber);

                if (seatExists == null) 
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var ticket = new Ticket
                {
                    Trip = trip,
                    CustomerCard = card,
                    Price = ticketDto.Price,
                    SeatingPlace = ticketDto.Seat
                };

                var origStation = trip.OriginStation.Name;
                var destStation = trip.DestinationStation.Name;
                var etd = trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                validTickets.Add(ticket);
                sb.AppendLine($"Ticket from {origStation} to {destStation} departing at {etd} imported.");
            }

            context.Tickets.AddRange(validTickets);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);

            return isValid;
        }
    }
}
