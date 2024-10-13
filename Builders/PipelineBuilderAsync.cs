// namespace Pipeline;
//
// public class PipelineBuilderAsync<TInput>
// {
//     private readonly Func<PipelineContext<TInput>, Task<object>> _currentPipeline;
//     private readonly IPipelineContextRepository _repository;
//
//     private PipelineBuilderAsync(Func<PipelineContext<TInput>, Task<object>> currentPipeline, IPipelineContextRepository repository)
//     {
//         _currentPipeline = currentPipeline;
//         _repository = repository;
//     }
//
//     // Initial constructor with repository
//     public PipelineBuilderAsync(IPipelineContextRepository repository) : this(async context => context.Data, repository) { }
//
//     // Method to add an asynchronous step to the pipeline
//     public PipelineBuilderAsync<TNext> AddStep<TNext>(IPipelineStepAsync<TInput, TNext> step)
//     {
//         if (step == null) throw new ArgumentNullException(nameof(step));
//
//         Func<PipelineContext<TInput>, Task<object>> newPipeline = async context =>
//         {
//             if (!context.IsSuccessful)
//             {
//                 return default;
//             }
//
//             var outputContext = await step.ProcessAsync(context);
//             await _repository.SaveContextAsync(outputContext); // Persist after step
//             return outputContext.Data;
//         };
//
//         // Transition the pipeline from TInput to TNext and return a new PipelineBuilderAsync<TNext>
//         return new PipelineBuilderAsync<TNext>(
//             async nextContext => await _currentPipeline((PipelineContext<TInput>)(object)nextContext), 
//             _repository
//         );
//     }
//
//     // Method to build the final asynchronous pipeline function
//     public Func<PipelineContext<TInput>, Task<TOutput>> Build<TOutput>()
//     {
//         return async context =>
//         {
//             object result = await _currentPipeline(context);
//             return (TOutput)result;
//         };
//     }
// }
