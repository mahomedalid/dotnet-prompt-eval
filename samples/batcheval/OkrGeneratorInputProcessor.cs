using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using System.Text;
using batcheval.Core;

namespace batcheval;

internal class OkrGeneratorInputProcessor : IInputProcessor<UserInput>
{
    private OkrGenerator okrGenerator;

    public OkrGeneratorInputProcessor(OkrGenerator okrGenerator)
    {
        this.okrGenerator = okrGenerator;
    }

     public async Task<ModelOutput> Process(UserInput userInput)
     {
        var title = await this.okrGenerator.GenerateTitle(userInput.Description);
        return new ModelOutput() { Output = title, Input = $"Objective title for {userInput.Description}" };
     }
}