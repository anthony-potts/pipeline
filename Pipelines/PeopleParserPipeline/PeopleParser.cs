using Microsoft.Extensions.Logging;
using Pipeline.Interfaces;
using Pipeline.Pipelines.Models;
using Pipeline.Pipelines.Steps;

namespace Pipeline.Pipelines;

public class PeopleParser : IPipelineRunner
{
    private readonly PipelineAuditor _pipelineAuditor;

    public PeopleParser(PipelineAuditor pipelineAuditor)
    {
        _pipelineAuditor = pipelineAuditor;
    }
    
    public void Run(string[] args)
    {
        // Define the storage path for PipelineContexts
        string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PipelineContexts");

        // Initialize the repository
        IPipelineContextRepository repository = new JsonFilePipelineContextRepository(storagePath);

        // Initialize the pipeline builder with input type string
        var pipelineBuilder = new PipelineBuilder<PersonPipelineContext>(repository);


        // Create a list of input data
        var inputData = new List<string>
        {
            "John Doe, 30",
            "Jane Smith, 25",
            "Invalid Data",
            "Bob Johnson, -5",
            "Alice Williams, 130",
            "Charlie Brown, 40"
        };


        pipelineBuilder.AddStep(new ParseStep(inputData)) // string -> Person
            .AddStep(new ValidateStep()) // Person -> Person
            .AddStep(new SerializeStep()); // Person -> string

        var pipelineRun = pipelineBuilder.Build<PersonPipelineContext>();

        // Prepare a list to hold successful outputs
        var successfulOutputs = new List<Person>();
        
        
        // Execute the batch pipeline
        var resultContext = pipelineRun(new PersonPipelineContext());


        // Process results

        if (resultContext.IsSuccessful)
        {
            foreach (var person in resultContext.People)
            {
                successfulOutputs.Add(person);
            }
        }
        else
        {
            // Assuming CorrelationId corresponds to the input
            Console.WriteLine($"Failed to process Input with CorrelationId: {resultContext.CorrelationId}");
            foreach (var error in resultContext.Errors)
            {
                Console.WriteLine($"  - {error}");
            }
        }

        Console.WriteLine(); // For readability


        // Display all successful outputs
        Console.WriteLine("All Successful Outputs:");
        foreach (var output in successfulOutputs)
        {
            Console.WriteLine(output);
        }
        
        //
        Console.WriteLine("output JSON");
        
        Console.WriteLine(resultContext.OutputList);

        _pipelineAuditor.OutputAudit(resultContext);
        
    }
}