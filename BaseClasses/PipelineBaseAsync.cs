// namespace Pipeline;
//
// public class PipelineBaseAsync<TInput, TOutput>
// {
//     private readonly Func<PipelineContext<TInput>, Task<TOutput>> _pipelineFunc;
//
//     internal PipelineBaseAsync(Func<PipelineContext<TInput>, Task<TOutput>> pipelineFunc)
//     {
//         _pipelineFunc = pipelineFunc;
//     }
//
//     public async Task<TOutput> ExecuteAsync(PipelineContext<TInput> context)
//     {
//         return await _pipelineFunc(context);
//     }
//
//     // Static method to initiate the pipeline building
//     public static PipelineBuilderAsync<TInput> Create()
//     {
//         // You need to provide a repository when creating the builder
//         throw new NotImplementedException("Use the overloaded Create method with a repository.");
//     }
//
//     public static PipelineBuilderAsync<TInput> Create(IPipelineContextRepository repository)
//     {
//         return new PipelineBuilderAsync<TInput>(repository);
//     }
// }
