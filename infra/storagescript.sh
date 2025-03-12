resourceGroupName="Management"
location="westeurope"
storageAccountName="terraformanagement"
containerName="tfstateblob"

az group create --name $resourceGroupName --location $location
az storage account create \
    --name $storageAccountName \
    --resource-group $resourceGroupName \
    --location $location \
    --sku Standard_LRS

echo "Storage account $storageAccountName created successfully."

accountKey=$(az storage account keys list \
    --resource-group $resourceGroupName \
    --account-name $storageAccountName \
    --query '[0].value' \
    --output tsv)
az storage container create \
    --name $containerName \
    --account-name $storageAccountName \
    --account-key $accountKey

echo "Storage container $containerName created successfully."