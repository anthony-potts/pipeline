using Pipeline.Interfaces;
using Pipeline.Pipelines.Steps;

namespace Pipeline.Pipelines;

public class PeopleParser: IPipelineRunner
{
    public void Run(string[] args)
    {
        // Define the storage path for PipelineContexts
        string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PipelineContexts");

        // Initialize the repository
        IPipelineContextRepository repository = new JsonFilePipelineContextRepository(storagePath);

        // Initialize the pipeline builder with input type string
        var pipelineBuilder = PipelineBase<string, string>.Create(repository)
            .AddStep(new ParseStep()) // string -> Person
            .AddStep(new ValidateStep()) // Person -> Person
            .AddStep(new SerializeStep()); // Person -> string

        // Build the pipeline function
        var pipelineFunc = pipelineBuilder.Build();

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

        // Prepare a list to hold PipelineContext<string> for each input
        var contexts = new List<PipelineContext<string>>();

        // Prepare a list to hold successful outputs
        var successfulOutputs = new List<string>();

        // Initialize BatchPipeline
        var batchPipeline = new BatchPipeline<string, string>(pipelineFunc);

        // Create PipelineContext for each input
        foreach (var input in inputData)
        {
            var context = new PipelineContext<string>
            {
                Data = input
            };
            contexts.Add(context);
        }

        // Execute the batch pipeline
        var results = batchPipeline.Execute(contexts);

        // Process results
        foreach (var resultContext in results)
        {
            if (resultContext.IsSuccessful)
            {
                successfulOutputs.Add(resultContext.Data);
                Console.WriteLine($"Success: {resultContext.Data}");
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
        }

        // Display all successful outputs
        Console.WriteLine("All Successful Outputs:");
        foreach (var output in successfulOutputs)
        {
            Console.WriteLine(output);
        }

        // Optionally, review the persisted PipelineContext JSON files in the 'PipelineContexts' directory
    }
}