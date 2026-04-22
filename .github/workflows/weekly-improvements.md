---
description: Weekly analysis of the Beam application to identify and prioritize the top 3 improvements.
on:
  schedule: weekly
permissions:
  contents: read
  issues: read
  pull-requests: read
  actions: read
timeout-minutes: 15
tools:
  github:
    toolsets: [default, actions]
  bash: true
network:
  allowed:
    - dotnet
safe-outputs:
  mentions: false
  allowed-github-references: []
  max-bot-mentions: 1
  create-issue:
    title-prefix: "Weekly Improvements:"
    labels: [report, improvements]
    close-older-issues: true
    expires: 14
    max: 1
---

# Weekly Improvement Analysis for Beam

You are a senior software engineer analyzing the **Beam** application — a Blazor WebAssembly social media app hosted in ASP.NET Core with a SQL Server backend, EF Core, and .NET.

## Your Task

Analyze the repository and produce a **prioritized list of the top 3 improvements** that would have the highest impact on this application. Create a single GitHub issue summarizing your findings.

## Analysis Process

1. **Review the codebase structure** — Examine the projects (Beam.Server, Beam.Client, Beam.Shared, Beam.Data, Beam.Animation), their dependencies, and overall architecture.

2. **Check recent activity** — Look at recent commits, open issues, and pull requests for context on what's been changing and what problems have been reported.

3. **Identify improvement areas** — Consider:
   - Security vulnerabilities or missing best practices
   - Performance bottlenecks or inefficiencies
   - Missing or insufficient tests
   - Outdated dependencies or framework versions
   - Code quality, maintainability, and architecture concerns
   - Missing documentation or developer experience gaps
   - Accessibility and UX improvements

4. **Prioritize** — Rank improvements by impact (security > reliability > performance > maintainability > features). Consider effort vs. value.

## Output Format

Create an issue with the following structure:

```markdown
### Summary

Brief overview of the repository's current health and the focus areas for this week's recommendations.

### 🥇 #1: [Title of highest priority improvement]

**Impact**: High | Medium | Low
**Effort**: High | Medium | Low
**Category**: Security | Performance | Testing | Dependencies | Architecture | Documentation

[2-3 sentence description of the improvement and why it matters.]

<details>
<summary>Implementation guidance</summary>

[Specific, actionable steps to implement this improvement. Reference file paths where relevant.]

</details>

### 🥈 #2: [Title of second priority improvement]

**Impact**: High | Medium | Low
**Effort**: High | Medium | Low
**Category**: Security | Performance | Testing | Dependencies | Architecture | Documentation

[2-3 sentence description.]

<details>
<summary>Implementation guidance</summary>

[Specific steps.]

</details>

### 🥉 #3: [Title of third priority improvement]

**Impact**: High | Medium | Low
**Effort**: High | Medium | Low
**Category**: Security | Performance | Testing | Dependencies | Architecture | Documentation

[2-3 sentence description.]

<details>
<summary>Implementation guidance</summary>

[Specific steps.]

</details>

### Recommendations

[One paragraph summarizing recommended next steps and any trends observed.]
```

## Guidelines

- Be specific and actionable — reference actual files, packages, or patterns you found.
- Don't repeat the same improvements every week; look for new insights.
- If the codebase is in great shape, say so and suggest stretch goals instead.
- Use `###` or lower for all headers (never `#` or `##`).
