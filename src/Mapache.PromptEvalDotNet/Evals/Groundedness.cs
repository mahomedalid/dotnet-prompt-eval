﻿using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Threading;

namespace Mapache.PromptEvalDotNet.Evals
{
    public class Groundedness
    {
        private readonly Counter<int> _counter;
        private readonly Histogram<int> _histogram;

        private readonly KernelPlugin _prompts;
        private readonly Kernel _kernel;

        public Groundedness(KernelPlugin _prompts, Kernel _kernel, Counter<int> _counter, Histogram<int> _histogram, string? promptId = null)
        {
            this._counter = _counter;
            this._histogram = _histogram;

            this._kernel = _kernel;
            this._prompts = _prompts;
        }

        public async Task<int> GetScore(string context, string answer)
        {
            var eval = _prompts["groundedness"];

            var args = new KernelArguments
            {
                { "Context", context },
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