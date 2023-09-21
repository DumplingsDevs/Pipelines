# ADR #003: Manage DI Scope in Dispatchers

**Date:** 2023-09-01

**Decision-makers:** Mateusz Wróblewski, Kamil Bytner

**Status:** Approved

## Context
Based on the architecture or coding approach chosen, we sometimes want handlers to run in isolation, meaning within their own scope. This is especially relevant for a modular monolith architecture, where each module has its own DI container, separate from the whole application.

For simpler applications where the entire DI container is shared, there's no need to set up an isolated DI scope, as this consumes additional resources.

## Problem
Our primary advantage is delivering a fully functional dispatcher implementation. If we don't provide the capability to create a scope, it would ultimately require the developer to add an extra implementation, which is not what we aim for.

## Considered Options
1. **Always create DI Scope during processing inputs:**
    - Pros: Simple to implement.
    - Cons: This is not always necessary, which could result in unnecessary resource consumption.

2. **Allow the decision of whether to create a DI Scope during input processing or not:**
    - Pros: DI Scope created only in dispatchers where it is necessary.
    - Cons: Harder to implement given the nature of the Source Generator.

## Selected Option
We chose the #2 option because we want to give developers as much freedom in use as possible

## Consequences
- Each pipeline can be configured whether to create a DI scope or not. This is not a global configuration for all pipelines, so it may happen that some dispatchers in the application will create a DI scope and some will not.

## Costs and Risks
- By default, a DI Scope is always created. If this is not the desired behavior and the developer does not read the documentation, he may be unaware of this pipeline behavior.

## Approved by
Mateusz Wróblewski, Kamil Bytner

**Approval Date:** 2023-05-29

**Links to other ADRs:** None
