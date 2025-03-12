# ASH Contribution Guide Overview

Welcome to the ASH Contribution Guide!

ASH, short for Avanade Support Hub, is a fully integrated, customizable chat solution that **connects customers with the right agents in real-time**. With smart routing, agent profiles, and a seamless widget for any website, Avanade Support Hub ensures fast, efficient, and personalized support experiences. This guide will help you learn everything you need to know to contribute to or launch this application.

The guide is broken up into sections that review each part of the contribution process, from setting up your PC to work with the ASH GitHub repo to writing your entry, creating a pull request, and implementing feedback from the ASH supervisors.

Good luck, and we look forward to seeing your contributions!

## Getting Started

New to GitHub? Get started with the following link:

- [Introduction to GitHub](https://github.com/skills/introduction-to-github)

## **How to Contribute**

### Prerequisites

- Have an existing [Microsoft Azure](https://portal.azure.com/) subscription
- An existing Communication Services resource (in this case, the chat resource added through the Azure portal)
  - See tutorial: [Quickstart - Create and manage resources in Azure Communication Services](https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp)
- Make sure you have [Node.js](https://nodejs.org/en) (v22.14.0)
- Also make sure to have [.NET SDK](https://dotnet.microsoft.com/en-us/download) (v9.0.102) (required for backend)
- An IDE (preferably Visual Studio Code)
- Have an existing GitHub account

### Setup Guidelines for contributing to GitHub

1. **Create your branch:**

   ```sh
   git checkout -b type/<#backlog_number>-<your_branch_name>
   ```

2. **Save your changes, then stage them:**

   ```sh
   git add <filename>
   ```

3. **Commit your changes:**

   ```sh
   git commit -m "<your-commit-message>"
   ```

4. **Push your changes to your branch:**

   ```sh
   git push origin "<your_branch_name>"
   ```

5. **Create a pull request:**

   1. After pushing your changes, go to your branch and click the "Compare & pull request" button. If you have multiples of this button, be sure you click the one for the correct branch.

   2. If you don't see this button, you can click the branch dropdown menu and then select the branch you just pushed from your local clone.

   3. Once you have switched to the correct branch on GitHub, click the "Contribute" dropdown and then click the "Open pull request" button.

   4. Give the PR a title and a description in the next window.

   5. Also, add a couple of reviewers (at least 2) to your PR.

   6. Then, click on the "Create pull request" button.

   7. At this point, a maintainer will either leave general comments, request changes, or approve and merge your PR.
      - It is important to respond to any comments or requested changes in a timely manner; otherwise, your PR may be closed without being merged due to inactivity.
      - After pushing any requested changes to the branch you opened the PR with, be sure to re-request a review from the maintainer that requested those changes at the top of the right sidebar.

More info at: https://docs.github.com/en/get-started/using-github/github-flow

## Commit Guidelines

### **Commit Message Guidelines 💬**

We follow a standardized commit message format to ensure consistency and clarity in our commit history. Each commit message should adhere to the following guidelines: https://www.conventionalcommits.org/en/v1.0.0/

## Code Conventions

To maintain code consistency and readability, we follow these conventions:

### General Guidelines

- Use camelCase for variable names and JavaScript/TypeScript function names (React, Node.js, Next.js).

- Use PascalCase for class names and C# (.NET) function names.

- Write meaningful comments for complex logic.

- Keep functions small and focused on a single task.

### React-Specific Guidelines

- Use functional components with hooks whenever possible.

- Keep components modular and reusable.

- Use React PropTypes or TypeScript for type checking.

- Use state management wisely (React Context, Redux, Zustand, or Recoil, based on project needs)..

### Backend (.NET) Guidelines

- Follow Clean Architecture principles.

- Use dependency injection instead of static classes.

- Keep API controllers lean by offloading logic to services.

- Use async/await for non-blocking operations.

### Team Preferences

- **Code Reviews:** PRs require at least two approvals before merging.

- **Unit Tests:** Ensure that new features include unit tests.

- **Documentation:** If you introduce a new feature, update the documentation accordingly.

- **Formatting:** Make sure that your code is well formatted (use tools like Prettier for JavaScript/TypeScript and SonarQube for .NET).

- **Branch Naming:** Use feature/, bugfix/, or hotfix/ prefixes based on the change type. (e.g., feature <backlog-number>chat-enhancements or bugfix/<backlog-number>fix-auth-issue).
