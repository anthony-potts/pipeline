// See https://aka.ms/new-console-template for more information

using Pipeline.Pipelines;
Console.WriteLine("=== Pipeline Execution Started ===\n");

// Instantiate the PeopleParser
var peopleParser = new PeopleParser();

// Run the PeopleParser pipeline
peopleParser.Run([string.Empty]);

Console.WriteLine("\n=== Pipeline Execution Completed ===");