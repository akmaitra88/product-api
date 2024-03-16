# ProductAPI
This is a product api build on .NET 6 and containerized using Docker.
- Build and Push the API image into ACR
- Deploy the image from ACR to AKS
- Using Helm chart for packaging deployment
```
helm create helm3
```
here helm3 is release name and chart name
Note: We will maintain all heml config files inside this release folder i.e. helm3 and delete those files which are not required for initial stages.

## GitHub Action
Repository Secrets
- ACR_REGISTRY_PASSWORD
- ACR_REGISTRY_USERNAME
- AZURE_CREDENTIALS
- REGISTRY_LOGIN_SERVER
- RESOURCE_GROUP
![Alt text](image.png)

## Infrastructure Set up
Here is step by step approach to create infrastructure in Azure
```
https://learn.microsoft.com/en-us/azure/container-instances/container-instances-github-action?tabs=userlevel

az login
az account show

# change the active subscription using the subscription name
az account set --subscription "My Demos"

# change the active subscription using the subscription ID
az account set --subscription "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"

groupId=$(az group show --name <resource-group-name> --query id --output tsv)
Resource-group-name = rg-dotnet-poc-01

az group show --name rg-dotnet-poc-01 --query id --output tsv
Output : /subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/resourceGroups/rg-dotnet-poc-01

az ad sp create-for-rbac --scope $groupId --role Contributor --json-auth

az ad sp create-for-rbac --name "AzServicePrincipal" --scope /subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/resourceGroups/rg-dotnet-poc-01 --role Contributor

{
  "appId": "e814a67c-f885-4246-a3aa-7a1b81521803",
  "displayName": "AzServicePrincipal",
  "password": "uiP8Q~0IRwMQANmV4DowANclsnZK8KTALZsvYbBX",
  "tenant": "36b9a392-41bd-4ef8-96f9-4ed6c8ed4118"
}

Save this JSON as repository secret into AZURE_CREDENTIALS
{
  "clientId": "e814a67c-f885-4246-a3aa-7a1b81521803",
  "clientSecret": "uiP8Q~0IRwMQANmV4DowANclsnZK8KTALZsvYbBX",
  "subscriptionId": "82accf95-d833-4be2-bc41-70c0ecaf3b99",
  "tenantId": "36b9a392-41bd-4ef8-96f9-4ed6c8ed4118",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}

Update the Azure service principal credentials to allow push and pull access to your container registry. This step enables the GitHub workflow to use the service principal to authenticate with your container registry and to push and pull a Docker image.

Get the resource ID of your container registry. Substitute the name of your registry in the following az acr show command
registryId=$(az acr show --name <registry-name> --resource-group <resource-group-name> --query id --output tsv)
az acr show --name privatepocacr01 --resource-group rg-dotnet-poc-01 --query id --output tsv

Output: /subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/resourceGroups/rg-dotnet-poc-01/providers/Microsoft.ContainerRegistry/registries/privatepocacr01

Use az role assignment create to assign the AcrPush role, which gives push and pull access to the registry. Substitute the client ID of your service principal:
az role assignment create --assignee <ClientId> --scope $registryId --role AcrPush

az role assignment create --assignee e814a67c-f885-4246-a3aa-7a1b81521803 --scope /subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/resourceGroups/rg-dotnet-poc-01/providers/Microsoft.ContainerRegistry/registries/privatepocacr01 --role AcrPush

Output: 
{
  "canDelegate": null,
  "condition": null,
  "conditionVersion": null,
  "description": null,
  "id": "/subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/resourceGroups/rg-dotnet-poc-01/providers/Microsoft.ContainerRegistry/registries/privatepocacr01/providers/Microsoft.Authorization/roleAssignments/43480938-9aaf-41da-9207-312782727baa",
  "name": "43480938-9aaf-41da-9207-312782727baa",
  "principalId": "cd45711f-0eb5-4f39-a9a7-31ce62d5bc38",
  "principalType": "ServicePrincipal",
  "resourceGroup": "rg-dotnet-poc-01",
  "roleDefinitionId": "/subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/providers/Microsoft.Authorization/roleDefinitions/8311e382-0749-4cb8-b61a-304f252e45ec",
  "scope": "/subscriptions/82accf95-d833-4be2-bc41-70c0ecaf3b99/resourceGroups/rg-dotnet-poc-01/providers/Microsoft.ContainerRegistry/registries/privatepocacr01",
  "type": "Microsoft.Authorization/roleAssignments"
}

Finally the command will connect the AKS with ACR and generates a kube.config file in user folder
az aks get-credentials --resource-group myResourceGroup --name myAKSCluster
az aks get-credentials --resource-group rg-dotnet-poc-01 --name myPOCAKSCluster

From <https://learn.microsoft.com/en-us/azure/aks/learn/quick-kubernetes-deploy-portal?tabs=azure-cli> 

kubectl get deployments -n api-dev-01

```