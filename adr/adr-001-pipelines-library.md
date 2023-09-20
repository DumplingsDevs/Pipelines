# ADR #001: Choice of Mediator Implementation in .NET

**Date:** 2023-05-29

**Decision-makers:** Mateusz Wróblewski, Kamil Bytner

**Status:** Approved

## Context
The Mediator pattern in .NET offers multiple implementation options, with two primary approaches being the pragmatic use of the MediatR library and the development of a custom mediator. This ADR addresses the challenges and considerations surrounding these approaches and introduces a third solution - the Pipelines library.

## Problem
The choice between MediatR and a custom mediator has been a recurring topic of discussion. MediatR has proven effective in many cases, but it comes with limitations, especially in complex architectures like Clean Architecture.

## Considered Options
1. **MediatR:**
    - Pros: Effective for simple applications, well-established library.
    - Cons: Violation of the Dependency Rule in both Application and Domain layers, global scope for the entire application, limited support for multiple communication pipelines.

2. **Custom Mediator:**
    - Pros: Customizability, potential efficiency in complex architectures.
    - Cons: Requires additional development effort, increased likelihood of errors.

3. **Pipelines Library:**
    - Pros: Offers flexibility to create multiple independent mediators tailored to specific use cases, does not impose specific types, maintains independence and decoupling among mediators, utilizes Source Generator for improved performance, supports decorators.
    - Cons: Learning curve and adoption of the Pipelines library.

## Selected Option
We have decided to choose the third option, which is the development of the Pipelines library. Our belief is that a good library should adapt to the application's needs, not the other way around. Pipelines provide developers with the freedom to create any number of mediators within their applications, each customized for specific use cases, ensuring maximum flexibility in programming.

## Consequences
- Developers using the Pipelines library can avoid violating the Dependency Rule, retaining control over dependencies.
- Each mediator created with Pipelines is independent and decoupled from others, enhancing maintainability.
- The library's use of Source Generator minimizes the use of reflection, resulting in improved performance.
- Decorator support enables the addition of various effects, such as validation, logging, and the ability to implement the Chain of Responsibility pattern.

## Costs and Risks
- Developers need to invest time in learning and adopting the Pipelines library, which may require additional effort.

## Other Considerations
- The decision to develop the Pipelines library is motivated by the belief that a library should adapt to the application's needs, offering freedom and flexibility in mediator creation.

## Approved by
Mateusz Wróblewski, Kamil Bytner

**Approval Date:** 2023-05-29

**Links to other ADRs:** None
