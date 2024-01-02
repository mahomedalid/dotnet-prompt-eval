# DotNet AI/ML Evaluator

## Overview

DotNet AI/ML Prompt Evaluator is a library written in C# for the .NET platform that facilitates the evaluation, testing, and validation of applications and systems using Large Language Models (LLMs). The library is designed to seamlessly integrate with various testing frameworks and pipelines, making it an ideal choice for incorporating AI/ML evaluations into your development and deployment workflows.

## Features

- **Evaluation and Testing**: The library provides a set of tools to evaluate and test applications and systems powered by LLMs. It enables users to assess the performance, accuracy, and robustness of their AI/ML solutions.

- **Integration with xUnit**: DotNet AI/ML Evaluator can be easily integrated with xUnit, a popular unit testing framework for .NET, allowing automated checks of AI/ML systems in Continuous Integration/Continuous Deployment (CICD) pipelines.

- **Polyglot and Batch Processing Support**: The library supports polyglot environments and batch processing, enabling users to evaluate and measure systems across various platforms and in batch operations.

- **Responsible AI Practices**: DotNet AI/ML Evaluator is committed to promoting Responsible AI practices. It encourages users to consider ethical implications, fairness, transparency, and accountability in the development and deployment of AI/ML systems.

- **Observability with System.Diagnostics.Metrics**: DotNet AI/ML Evaluator leverages the System.Diagnostics.Metrics APIs for observability of metrics and traces in .NET applications. This allows users to instrument their applications, track important metrics, and gain insights into the performance and behavior of their AI/ML systems.

- **Integration with OpenTelemetry for Observability**: DotNet AI/ML Evaluator seamlessly integrates with OpenTelemetry, a set of APIs, libraries, agents, instrumentation to provide observability in your applications. This enables exporting metrics, traces, and logs to various backends such as Grafana, Prometheus, and more. Users can easily create dashboards and gain comprehensive insights into the behavior and performance of their AI/ML systems.

## Why DotNet AI/ML Evaluator?

- **Increased Awareness in the .NET Ecosystem**: By choosing DotNet AI/ML Evaluator, you contribute to the growth of tools and awareness within the .NET C# ecosystem for AI/ML systems. This helps in building a vibrant and diverse set of tools and resources for developers working with LLMs.

## Getting Started

1. **Installation**: Include the DotNet AI/ML Evaluator library in your .NET project using NuGet package manager.

   ```bash
   dotnet add package Mapache.PromptEvalDotNet
   ```

2. **Usage**: Incorporate the library into your testing scenarios, whether using xUnit, polyglot environments, or batch processing.

   ```csharp
   // Sample code for using DotNet AI/ML Evaluator
   var kernelBuilder = Kernel.CreateBuilder();

   kernelBuilder.AddAzureOpenAIChatCompletion(config["AZURE_OPENAI_MODEL"]!, config["AZURE_OPENAI_ENDPOINT"]!, config["AZURE_OPENAI_KEY"]!);
   
   var evalFactory = new Mapache.PromptEvalDotNet.EvaluatorFactory("MyProjectEvals", builder.Build());

   var coherence = await evalFactory.Coherence("blog-posts").GetScore(prompt, answer);
   ```

## Integration with xUnit

1. **Installation**: Include the DotNet AI/ML Evaluator library in your xUnit .NET project using NuGet package manager.

   ```bash
   dotnet add package Mapache.PromptEvalDotNet
   ```

2. **Fixture**: Create a Fixture, so the kernel and factory does not get created each time we have a test. You can create a fixture per assembly, class or collection. Example:

   ```csharp
   // Sample xUnit class for fixture
   public class PromptEvalDotNetFixture
   {
       public Mapache.PromptEvalDotNet.EvaluatorFactory EvaluatorFactory;

       public Kernel Kernel { get; }

       public PromptEvalDotNetFixture()
       {
           var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();
   
           var config = configurationBuilder.Build();
   
           var builder = Kernel.CreateBuilder();
   
           builder.AddAzureOpenAIChatCompletion(
                config["AZURE_OPENAI_MODEL"]!,
                config["AZURE_OPENAI_ENDPOINT"]!,
                config["AZURE_OPENAI_KEY"]!);
   
           Kernel = builder.Build();
   
           EvaluatorFactory = new Mapache.PromptEvalDotNet.EvaluatorFactory("MyProjectTests", Kernel);
       }
   }
   ```

3. **Configure the test class**: Add the fixture to your test class. You could use xUnit DI. Example:

   ```csharp
    public class MyPromptTests : IClassFixture<PromptEvalDotNetFixture>
    {
        private PromptEvalDotNetFixture _promptEvaluator;

        private readonly ITestOutputHelper _output;

        public MyPromptTests(PromptEvalDotNetFixture fixture, ITestOutputHelper output)
        {
            _promptEvaluator = fixture;
            _output = output;
        }
   }
   ```

4. **Use PromptEvalDotNet**:

```csharp
   var coherence = await factory
    .Coherence("support-chat")
    .GetScore(question, answer);

   var groundedness = await factory
       .GetGroundednessEvaluator("support-chat")
       .GetScore(question, answer);
   
   var relevance = await factory
       .GetRelevanceEvaluator("support-chat")
       .GetScore(question, answer, contextTopic);
   
   Assert.Multiple(
       () => Assert.True(coherence >= 3, $"Coherence the answer to {question} - score {coherence}"),
       () => Assert.True(groundedness >= 3, $"Groundedness the answer to {question} - score {groundedness}"),
       () => Assert.True(relevance >= 3, $"Relevance of the answer to {question} - score {relevance}")
   );
```

## Responsible AI Guidelines

1. **Ethical Considerations**: When using DotNet AI/ML Evaluator, consider the ethical implications of your AI/ML models and the potential impact on users and society.

2. **Fairness and Bias Mitigation**: Strive to mitigate bias and ensure fairness in AI/ML systems. Use diverse and representative datasets to avoid unintentional biases.

3. **Transparency**: Provide clear documentation and explanations for the evaluation criteria, metrics, and decision-making processes involved in your AI/ML evaluations.

4. **Accountability**: Establish mechanisms for accountability and responsibility in the development, deployment, and maintenance of AI/ML systems.

## Contributing

We welcome contributions from the community to enhance the features, documentation, and overall usability of DotNet AI/ML Evaluator. Feel free to open issues, submit pull requests, or engage in discussions.

## License

This project is licensed under the [MIT License](LICENSE.md).
