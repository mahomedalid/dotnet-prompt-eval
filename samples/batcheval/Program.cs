using batcheval.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using System.Text;

namespace batcheval;

class Program
{
    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddEnvironmentVariables()
            .Build();
    }

    private static Kernel CreateAndConfigureKernel(IConfiguration config)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(
            config["AZURE_OPENAI_MODEL"]!,
            config["AZURE_OPENAI_ENDPOINT"]!,
            config["AZURE_OPENAI_KEY"]!);

        return builder.Build();
    }

    static async Task Main()
    {
        // STEP 1: load the confuguration
        var config = GetConfiguration();
        var kernel = CreateAndConfigureKernel(config);

        // STEP 2: load the file
        var fileName = "assets/data.jsonl";

        Console.WriteLine($"Reading {fileName}, press a key to continue ...");
        Console.ReadLine();

        var kernelFunctions = kernel.CreatePluginFromPromptDirectory("Prompts");

        var batchEval = new BatchEval<UserInput>();

        batchEval
            .AddEvaluator(new PromptScoreEval("coherence", kernel, kernelFunctions["coherence"]))
            .AddEvaluator(new LenghtEval());

        await batchEval
            .WithInputProcessor(new OkrGeneratorInputProcessor(new OkrGenerator(kernel, kernelFunctions["okrtitle"])))
            .WithJsonl(fileName)
            .Run();

        Console.WriteLine($"Complete.");
        Console.ReadLine();
    }
}
