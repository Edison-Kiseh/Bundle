# Avanade Support Hub (ASH)

Welcome to the **Avanade Support Hub (ASH)**! ASH, short for Avanade Support Hub, is a fully integrated, customizable chat solution that **connects customers with the right agents in real-time**. With smart routing, agent profiles, and a seamless widget for any website, Avanade Support Hub ensures fast, efficient, and personalized support experiences. This guide will help you learn everything you need to know to contribute to or launch this application.

This guide will help you set up the full-stack application on your local machine for development and testing.

## Prerequisites

Ensure you have the following installed before proceeding:

- [**Microsoft Azure**](https://portal.azure.com/) account and an **Azure Communication Services** resource.
  - [Create and manage resources in Azure Communication Services](https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp)
- [**Node.js**](https://nodejs.org/en/) (v22.14.0)
- [**.NET SDK**](https://dotnet.microsoft.com/en-us/download) (v9.0.102) (required for backend)
- [**Git**](https://git-scm.com/) (for cloning the repository)
- **An IDE** (e.g., [Visual Studio Code](https://code.visualstudio.com/))

---

## Getting Started

### Step 1: Clone the Repository

Open a terminal and run the following command to clone the repository:

```bash
git clone https://github.com/Avanade-Region-Gallia/be-fy25-internship.git
cd be-fy25-internship
```

---

## Backend Setup (C# & .NET API)

### Step 2: Navigate to the Backend Directory

```bash
cd backend/ASH
```

### Step 3: Build the Backend Project

```bash
dotnet build
```

### Step 4: Run the API

```bash
dotnet watch
```

This will start the backend API and open the index page on `http://localhost:5254`.

---

## Frontend Setup (React & JavaScript)

### Step 5: Navigate to the Frontend Directory

In a new terminal, navigate to the frontend chat directory:

```bash
cd ../../chat
```

### Step 6: Install Frontend Dependencies

```bash
npm install
```

### Step 7: Create a .env file

In this file you need to put your secret values following the example of .env.Example

### Step 7: Run the Frontend Application

```bash
npm run dev
```

Your application should now be running at `http://localhost:3000` (or another available port).

---

## Configuration

### Step 8: Set Up Environment Variables

The `appsettings.Example.json` file contains sample values for your application’s environment settings. To configure the application, follow these steps:

1. **Copy the Example Configuration File**

Copy the contents of `appsettings.Example.json` to a new file called `appsettings.Development.json` (or another file, depending on your environment setup):

```bash
cp appsettings.Example.json appsettings.Development.json

```

2. **Update Configuration Values**

Open the newly created appsettings.Development.json file and replace the placeholder values with your actual credentials from Azure Communication Services. For example:

```json
{
  "CommunicationServices": {
    "ResourceConnectionString": "<YOUR_CONNECTION_STRING>",
    "EndpointUrl": "<YOUR_ENDPOINT_URL>",
    "AdminUserId": "<YOUR_ADMIN_USER_ID>"
  }
}
```

- <YOUR_CONNECTION_STRING>: Replace with your Azure Communication Services resource connection string.
- <YOUR_ENDPOINT_URL>: Replace with the endpoint URL for your Azure Communication Services.
- <YOUR_ADMIN_USER_ID>: Replace with the admin user ID you want to use for managing the service.

3. **Save the File**

After replacing the placeholders, save the appsettings.Development.json file.

This will ensure that the application can properly connect to Azure Communication Services using the provided credentials.

---

## Running Tests

### Frontend Tests

To run frontend tests, navigate to the test directory and run:

```bash
cd frontend/chat/__tests__
npm test
```

To test a specific file:

```bash
npm test --filter <test-filename>
```

### Backend Tests (Unit & Integration)

Navigate to the appropriate test directory for backend tests:

```bash
cd backend/ASH.Tests      # For unit tests
cd backend/ASH.IntegrationTests  # For integration tests
```

Run the tests with:

```bash
dotnet test
```

To test a specific file:

```bash
dotnet test --filter <test-filename>
```

---

## Troubleshooting

### 1️⃣ `dotnet watch` Not Starting

If the backend API doesn’t start, ensure you have the correct .NET SDK installed:

```bash
dotnet --version
```

If outdated, install the latest version from [Microsoft .NET](https://dotnet.microsoft.com/en-us/download).

### 2️⃣ `npm install` Fails Due to Dependency Issues

Try the following steps to resolve issues:

```bash
rm -rf node_modules package-lock.json && npm install
```

Or clear the npm cache:

```bash
npm cache clean --force
```

---

## Next Steps

🎉 **Congratulations!** Your ASH application should now be running locally.

If you'd like to contribute, check out the **[ASH Contribution Guide](CONTRIBUTING.md)** for instructions on how to make changes and submit pull requests.

---

## Additional Resources

- [Azure Communication Services Documentation](https://learn.microsoft.com/en-us/azure/communication-services/)
- Node.js Documentation
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)

---

## Code quality summary

### Quality & Maintainability

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=alert_status&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=sqale_rating&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=sqale_index&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=ncloc&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship)

### Code Issues & Smells

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=bugs&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=code_smells&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=duplicated_lines_density&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship)

### Security & Reliability

[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=security_rating&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=reliability_rating&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Avanade-Region-Gallia_be-fy25-internship&metric=vulnerabilities&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship)

### SonarCloud Summary

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=Avanade-Region-Gallia_be-fy25-internship&token=1b4eded9e6bee6f4d088febb6ba0df84232c1599)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship) [![SonarQube Cloud](https://sonarcloud.io/images/project_badges/sonarcloud-light.svg)](https://sonarcloud.io/summary/new_code?id=Avanade-Region-Gallia_be-fy25-internship)
