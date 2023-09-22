# ADR #003: Manage DI Scope in Dispatchers

**Date:** 2023-09-11

**Decision-makers:** Mateusz Wróblewski, Kamil Bytner

**Status:** Approved

## Context
In certain architectural setups or code configurations, we want our handlers to operate in their own scope. This is particularly important for a modular monolith structure, where each module has its distinct DI container, independent from the entire application. But in simpler applications with a shared DI container, creating isolated scopes might not be necessary.

## Problem
Our primary advantage is delivering a fully functional dispatcher implementation. If we don't provide the capability to create a scope, it would ultimately require the developer to add an extra implementation, which is not what we aim for.

## Considered Options
1. **Always create DI Scope during processing inputs:**
    - Pros: Straightforward for us to implement.
    - Cons: This is not always necessary, which could result in unnecessary resource consumption.

2. **Allow the decision of whether to create a DI Scope during input processing or not:**
    - Pros: DI Scope is created only when required.
    - Cons: This approach is somewhat more complex for us to develop.

## Selected Option
We opted for the second option. We believe in providing flexibility to developers in key feature of `Pipelines`.

## Consequences
- Each pipeline can be individually set to create own DI scope or not. As a result, certain dispatchers within the application might create own DI scope, while others might not.

## Costs and Risks
- A DI Scope is created automatically by default. Developers who might skip the documentation could miss this, leading to potential surprises in pipeline operation.

## Approved by
Mateusz Wróblewski, Kamil Bytner

**Approval Date:** 2023-09-11

**Links to other ADRs:** None
