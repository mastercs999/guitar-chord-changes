using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GuitarChordChanges
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load configuration
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Settings settings = config.GetSection(nameof(Settings)).Get<Settings>();

            // Load history
            ChordChange[] changes = File.Exists(settings.ChangesFile) ? JsonSerializer.Deserialize<ChordChange[]>(File.ReadAllText(settings.ChangesFile)) : new ChordChange[0];
            Dictionary<(string, string), ChordChange> chordsToChanges = changes.ToDictionary(x => (x.Chord1, x.Chord2));

            // Now to cartesian product of chord changes
            List<(string, string)> combinations = settings
                .KnownChords
                .SelectMany(x => settings.KnownChords, (x, y) => new string[] { x, y }.OrderBy(x => x).ToArray())
                .Select(x => (x[0], x[1]))
                .Where(x => x.Item1 != x.Item2)
                .Distinct()
                .ToList();

            // Create list of chord changes
            List<ChordChange> chordChanges = combinations.Select(x => chordsToChanges.TryGetValue(x, out ChordChange v) ? v : new ChordChange() { Chord1 = x.Item1, Chord2 = x.Item2 }).ToList();

            // Find out chord changes with lowest number
            foreach (ChordChange change in chordChanges.OrderBy(x => !x.ChordChangeRecords.Any() ? 0 : x.ChordChangeRecords.OrderByDescending(x => x.DateTime).Select(x => x.Count).Take(settings.AverageLookback).Average()).Take(settings.PracticeCount))
            {
                Console.Write($"{change.Chord1}-{change.Chord2} ({Math.Round(!change.ChordChangeRecords.Any() ? 0 : change.ChordChangeRecords.OrderByDescending(x => x.DateTime).Select(x => x.Count).Take(settings.AverageLookback).Average())}): ");
                int count = int.Parse(Console.ReadLine());
                change.ChordChangeRecords.Add(new ChordChangeRecord() { Count = count, DateTime = DateTime.Now });
            }

            // Save
            File.WriteAllText(settings.ChangesFile, JsonSerializer.Serialize(chordChanges, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
