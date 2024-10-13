using Pipeline.Interfaces;
using Pipeline.Pipelines.OrderParserPipeline.Steps;

namespace Pipeline.Pipelines.OrderParserPipeline;

 public class OrdersParser : IPipelineRunner
    {
        public void Run(string[] args)
        {
            // Define the storage path for PipelineContexts
            string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PipelineContexts");

            // Initialize the repository
            IPipelineContextRepository repository = new JsonFilePipelineContextRepository(storagePath);

            // Initialize the pipeline builder with input type string
            var pipelineBuilder = Pipeline<string, string>.Create(repository)
                .AddStep(new ParseOrderStep())      // string -> Order
                .AddStep(new ValidateOrderStep())   // Order -> Order
                .AddStep(new SerializeOrderStep()); // Order -> string

            // Build the pipeline function (non-generic)
            var pipelineFunc = pipelineBuilder.Build();

            // Create a list of input data
            var inputData = new List<string>
            {
                "Order001, Widget, 10",
                "Order002, Gadget, 5",
                "Invalid Order",
                "Order003, , 15",
                "Order004, Thingamajig, -3",
                "Order005, Doohickey, 20"
            };

            // Prepare a list to hold PipelineContext<string> for each input
            var contexts = new List<PipelineContext<string>>();

            // Prepare a list to hold successful outputs
            var successfulOutputs = new List<string>();

            // Initialize BatchPipeline with the correct Pipeline class
            var batchPipeline = new BatchPipeline<string, string>(new Pipeline<string, string>(pipelineFunc));

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
                    Console.WriteLine($"Failed to process Order with CorrelationId: {resultContext.CorrelationId}");
                    foreach (var error in resultContext.Errors)
                    {
                        Console.WriteLine($"  - {error}");
                    }
                }

                Console.WriteLine(); // For readability
            }

            // Display all successful outputs
            Console.WriteLine("All Successful Order Outputs:");
            foreach (var output in successfulOutputs)
            {
                Console.WriteLine(output);
            }

            // Optionally, review the persisted PipelineContext JSON files in the 'PipelineContexts' directory
        }
    }