// namespace Pipeline;
//
// public class BatchPipelineAsync<TInput, TOutput>
// {
//     private readonly Func<PipelineContext<TInput>, Task<PipelineContext<TOutput>>> _pipelineFunc;
//
//     public BatchPipelineAsync(Func<PipelineContext<TInput>, Task<PipelineContext<TOutput>>> pipelineFunc)
//     {
//         _pipelineFunc = pipelineFunc;
//     }
//
//     public async Task<List<PipelineContext<TOutput>>> ExecuteAsync(IEnumerable<PipelineContext<TInput>> contexts)
//     {
//         var resultList = new List<PipelineContext<TOutput>>();
//
//         foreach (var context in contexts)
//         {
//             var result = await _pipelineFunc(context);
//             resultList.Add(result);
//         }
//
//         return resultList;
//     }
// }