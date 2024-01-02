using Mapache.PromptEvalDotNet.Evals;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

namespace Mapache.PromptEvalDotNet
{
    public class EvaluatorFactory
    {
        private readonly MeterProvider _meterProvider;

        private readonly Kernel _kernel;

        private readonly KernelPlugin _kernelFunctions;
        
        public Meter Meter { get; }

        public string MeterId { get; }

        public EvaluatorFactory(string meterId, Kernel kernel)
        {
            this.MeterId = meterId;
            this._kernel = kernel;

            this.Meter = new(MeterId ?? "PromptEvalDotNet");

            _meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter(MeterId)
                .Build();

            _kernelFunctions = _kernel.CreatePluginFromPromptDirectory("Prompts");
        }

        public Coherence Coherence(string promptId)
        {
            return GetCoherenceEvaluator(promptId);
        }

        public Coherence GetCoherenceEvaluator(string promptId)
        {
            var counter = Meter.CreateCounter<int>($"coherence.counter.{promptId}");
            var histogram = Meter.CreateHistogram<int>($"coherence.{promptId}");
            
            return new Coherence(_kernelFunctions, _kernel, counter, histogram, promptId);
        }

        public Groundedness GetGroundednessEvaluator(string promptId)
        {
            var counter = Meter.CreateCounter<int>($"groundedness.counter.{promptId}");
            var histogram = Meter.CreateHistogram<int>($"groundedness.{promptId}");

            return new Groundedness(_kernelFunctions, _kernel, counter, histogram, promptId);
        }

        public Relevance GetRelevanceEvaluator(string promptId)
        {
            var counter = Meter.CreateCounter<int>($"relevance.counter.{promptId}");
            var histogram = Meter.CreateHistogram<int>($"relevance.{promptId}");

            return new Relevance(_kernelFunctions, _kernel, counter, histogram, promptId);
        }
    }
}