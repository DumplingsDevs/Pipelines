[![CI-main](https://github.com/DumplingsDevs/Pipelines/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/DumplingsDevs/Pipelines/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/DumplingsDevs.Pipelines.svg)](https://www.nuget.org/packages/DumplingsDevs.Pipelines/)

-----

<p align="center">
  <img src="docs/assets/pipelines_purple.svg#gh-light-mode-only" alt="Pipelines"/>
  <img src="docs/assets/pipelines_white.svg#gh-dark-mode-only" alt="Pipelines"/>
</p>



🛠 ```We believe that a good library should adapt to your application, rather than forcing your application to adapt to it. We recognize that every software project is unique and may employ different architectural patterns and designs. With Pipelines, we found a library that enables us to build our applications around our preferred patterns, rather than constraining us to rigid structures imposed by external libraries. This flexibility not only streamlines our development process but also empowers us to make design choices that best suit our specific use cases.```

💡 ```Pipelines empowers developers to leverage the potential of the Mediator pattern seamlessly, thanks to its flexible and adaptable foundations.```

-----

# 📦 Installation
```
dotnet add package DumplingsDevs.Pipelines
dotnet add package DumplingsDevs.Pipelines.WrapperDispatcherGenerator
```

----

# 📚 Documentation

The [Quick Start](docs/quick_start.md) section will allow you to quickly create your first Pipeline.

If you prefer learning through real examples, please check out the examples below:
- [Multistep process](docs/process_pipeline.md)
- [Commands, Queries and Domain Events Dispatching](docs/command_queries_events_example.md)

If you want to learn more, please read the articles below.

- [Main Concepts](docs/main_concepts.md)
- [Pipeline Design Guidelines](docs/pipeline_design_guidelines.md)
- [Configuration](docs/configuration.md)
- [Pipeline Cookbook](docs/pipeline_cookbook.md)
- [Troubleshooting](docs/troubleshooting.md)
- [Proxy vs Generated Dispatcher](docs/dispatcher_source_generator.md)
- [Benchmarks](docs/benchmarks.md)
- [ADR](docs/adr.md)

---- 

# ⚠️ Limitations
- Pipelines in which multiple handlers will be handled for one input must have a `Task` or `void` return type.
- Cannot create a Pipeline that returns both generic and non-generic types.

-----

# 🛤 Roadmap
- [ ] **ADR Documentation**: Record key architectural decisions made during implementation using ADRs.
- [ ] **Code Cleanup**: Refine and tidy up the codebase post-MVP, paving the way for new feature development.
- [ ] **Support for Nullable Results**: Add functionality to handle nullable result types.
- [v] **Configurable Dispatcher Behavior**: Implement configuration settings to decide if the dispatcher should throw an exception in case there's no handler for a given Input.
- [v] **Dependency Injection Scope Choice**: Provide an option to decide whether or not to create a Dependency Injection Scope in Dispatchers.
- [ ] **Multiple Inputs in Dispatcher**: Enhance the dispatcher handle method to accept a list of inputs instead of just one.
- [ ] **Parallel Pipeline**: Introduce a pipeline to facilitate parallel execution of multiple handlers.
- [ ] **Stream Pipeline**: Implement support for streaming pipelines.
- [ ] **Decorator Performance Optimization**: Improve performance, especially concerning the use of `ActivatorUtilities.CreateInstance()`.

-----

# 🥟💡 The Dumplings Behind the Magic
Hey there! We're Dumplings Devs, made up of <a href="https://pl.linkedin.com/in/matwroblewski">Mateusz Wróblewski</a> and <a href="https://pl.linkedin.com/in/kamil-bytner">Kamil Bytner</a>. We're passionate about software and always up for a coding challenge. 

---

<p align="center">
  <img src="docs/assets/dumplings_purple.svg#gh-light-mode-only" alt="DumplingsDevs"/>
  <img src="docs/assets/dumplings_white.svg#gh-dark-mode-only" alt="DumplingsDevs"/>
</p>
