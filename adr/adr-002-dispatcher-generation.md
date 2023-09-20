# ADR #002: Choice of Tool for Creating the Dispatcher

**Date:** 2023-06-11

**Decision-makers:** Mateusz Wróblewski, Kamil Bytner

**Status:** Approved

## Context

The pipelines library is a versatile library for creating various mediator patterns based on user-defined types. To
achieve this, we need to prepare an implementation of the Dispatcher, which will find the Handler based on the Input in
some way.

## Problem

We considered two main options: using the base DispatchProxy class, which acts as a proxy for the given interface, and
using a Source Generator, which can generate the Dispatcher during build based on the Pipelines configuration.

## Considered Options

1. **Use DispatchProxy:**
    - Pros: Simple to use, does not require code generation during compilation.
    - Cons: Slower solution, relies on reflection.

2. **Use Source Generator:**
    - Pros: More efficient solution, code generation during compilation, better type control.
    - Cons: Higher risk of errors, higher entry threshold.

## Selected Option

We have chosen option #2, which is to use a Source Generator. Despite the higher risk of errors, we believe it provides
better performance and allows for better control over the types created by the user. Additionally, we plan to add
configuration validators to minimize the risk of errors.

## Consequences

- Choosing the Source Generator will impact the performance and type control in our library.
- We will need to ensure thorough testing and monitoring of the generated code.
- Adding configuration validators will be crucial for ensuring the correctness of the Source Generator.

## Costs and Risks

- Increased risk of errors in the generated code.
- Higher entry threshold.

## Other Considerations

- We didn't find any other options to handle that use case

## Approved by

Mateusz Wróblewski, Kamil Bytner

**Approval Date:** 2023-06-11

**Links to other ADRs:** None
