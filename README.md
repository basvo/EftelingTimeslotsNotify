# About EftelingTimeslotsNotify
Notifies a user when a timeslot for the Efteling theme park becomes available. For now it's only an Azure Function that reads the data from the Efteling website and stores it in a CosmosDB collection. I'm still working on the other components that will eventually notify the user.

# Prerequisites

It is recommended to use [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) or [VS Code](https://code.visualstudio.com/).

A CosmosDB instance should be configured in your Azure subscription.

# Running Locally

You should configure a local.settings.json file to hold the following values:

```json
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "WebscrapeURL": "https://www.efteling.com/nl/park/reserveer-bezoek/abonnementhouders/beschikbare-tijdsloten",
    "CosmosDBConnection": "<Your primary connection string from Cosmos DB>"
  }
}
```
