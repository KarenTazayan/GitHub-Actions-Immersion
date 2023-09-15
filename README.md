## GitHub Actions Immersion

### 1. Use GitHub Actions to connect to Azure.

Use your existing Microsoft Azure Subscription, [you can create a free account](https://azure.microsoft.com/en-us/free/) if you don't have any.  
By using Azure CLI create a service principal and configure its access to Azure resources. To retrieve current subscription ID, run:  
```
az account show --query id --output tsv
```
Configure its access to Azure subscription:
```
az ad sp create-for-rbac --name GitHub-Actions-Immersion-1 --role Owner --scopes /subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```
Copy the JSON object for your service principal:
```
{
  "appId": "<GUID>",
  "displayName": "GitHub-Actions-Immersion-1",
  "password": "client secret",
  "tenant": "<GUID>"
}
```
Make a new JSON object with values from the previous object:
```
{
    "clientId": "Replace with appId value",
    "clientSecret": "Replace with password value",
    "subscriptionId": "Put the Azure subscribtion Id here",
    "tenantId": "Replace with tenant value"
}
```
Add the JSON [as a GitHub secret](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure) with replaced values (GitHub-Actions-Immersion-1), required name: **AZURE_CREDENTIALS**.