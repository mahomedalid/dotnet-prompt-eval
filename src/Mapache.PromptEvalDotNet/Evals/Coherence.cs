using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Threading;

namespace Mapache.PromptEvalDotNet.Evals
{
    public class Coherence
    {
        private readonly Counter<int> _counter;
        private readonly Histogram<int> _histogram;

        private readonly KernelPlugin _prompts;
        private readonly Kernel _kernel;

        public Coherence(KernelPlugin prompts, Kernel kernel, Counter<int> counter, Histogram<int> histogram, string? promptId = null)
        {
            _counter = counter;
            _histogram = histogram;

            _kernel = kernel;
            _prompts = prompts;
        }

        public async Task<int> GetScore(string question, string answer)
        {
            var eval = _prompts["coherence"];

            var args = new KernelArguments
            {
                { "Question", question },
                { "Answer", answer }
            };

            _counter.Add(1);

            var evalResult = await eval.InvokeAsync(_kernel, args);

            var evalInt = int.Parse(evalResult.ToString());

            _histogram.Record(evalInt);

            return evalInt;
        }
    }
}