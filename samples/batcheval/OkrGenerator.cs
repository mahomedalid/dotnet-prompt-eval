using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using System.Text;

namespace batcheval;

internal class OkrGenerator
{
    private Kernel kernel;

    private KernelFunction function;

    public OkrGenerator(Kernel kernel, KernelFunction function)
    {
        this.function = function;
        this.kernel = kernel;
    }

    public async Task<string> GenerateTitle(string input)
    {
       var promptArgs = new KernelArguments
            {
                { "Input", input }
            };

        var result = await function.InvokeAsync(kernel, promptArgs);

        return result.ToString();
    }
}