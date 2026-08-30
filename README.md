[![](https://img.shields.io/nuget/v/soenneker.copper.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.copper.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.copper.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.copper.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.copper.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.copper.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.copper.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.copper.openapiclient/actions/workflows/codeql.yml)

# Soenneker.Copper.OpenApiClient

A Kiota-generated .NET client for Copper's Developer API, generated from Copper's published Postman collection.

## Install

```bash
dotnet add package Soenneker.Copper.OpenApiClient
```

## Recommended setup

For dependency injection, API-key headers, and client reuse, install the companion utility:

```bash
dotnet add package Soenneker.Copper.OpenApiClientUtil
```

```csharp
using Soenneker.Copper.OpenApiClientUtil.Registrars;

services.AddCopperOpenApiClientUtilAsSingleton();
```

Configure `Copper:ApiKey` and `Copper:Email`, then inject `ICopperOpenApiClientUtil` and call `Get`.

## Direct construction

Copper API-key requests require three headers:

```csharp
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Copper.OpenApiClient;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.copper.com/developer_api/v1/")
};
httpClient.DefaultRequestHeaders.Add("X-PW-AccessToken", apiKey);
httpClient.DefaultRequestHeaders.Add("X-PW-Application", "developer_api");
httpClient.DefaultRequestHeaders.Add("X-PW-UserEmail", tokenOwnerEmail);

var authentication = new AnonymousAuthenticationProvider();
var adapter = new HttpClientRequestAdapter(authentication, httpClient: httpClient);
var copper = new CopperOpenApiClient(adapter);

string? accountJson = await copper.Account.GetAsync(cancellationToken: cancellationToken);
```

`AnonymousAuthenticationProvider` is appropriate because this dedicated `HttpClient` already carries Copper's required headers. Never put Copper credentials on a client that can send default headers to unrelated hosts.

## Navigating the client

Root request builders include `Account`, `People`, `Companies`, `Leads`, `Opportunities`, `Projects`, `Tasks`, `Users`, `Activities`, and `Webhooks`. Item and search operations are exposed beneath those resources according to the generated hierarchy.

The source Postman collection does not describe every response with a strong schema. Some generated methods therefore return `string?` containing JSON rather than a model. Inspect the method's declared return type and deserialize primitive JSON responses in application code when necessary.

## Practical notes

- Keep the `HttpClient`, request adapter, and generated client long-lived. The companion utility manages that lifecycle for dependency-injection applications.
- Generated names preserve source identifiers, including members such as `Activity_types` and `Custom_field_definitions`.
- Endpoint results can be nullable. Copper errors without generated schemas may surface as Kiota or HTTP exceptions rather than typed error models.
- Public members and return types can change when the Postman source is converted and the client is regenerated.
- Files under `src/Soenneker.Copper.OpenApiClient` are generated. Keep application-specific behavior outside the generated project.
- Treat the API key and token-owner email as credentials and redact all three Copper authentication headers from logs and traces.
