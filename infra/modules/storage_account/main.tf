resource "azurerm_resource_group" "internship" {
  name     = "internship-resources"
  location = "West Europe"
}
resource "azurerm_storage_account" "companymediastorage2025" {
  name                     = "companymediastorage2025"
  resource_group_name      = azurerm_resource_group.internship.name
  location                 = azurerm_resource_group.internship.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "chatcontainer" {
  name                  = "chatcontainer"
  storage_account_id    = azurerm_storage_account.companymediastorage2025.id
  container_access_type = "private"
}

resource "azurerm_storage_blob" "chatlogsblob" {
  name                   = "logs"
  storage_account_name   = azurerm_storage_account.companymediastorage2025.name
  storage_container_name = azurerm_storage_container.chatcontainer.name
  type                   = "Block"
  source                 = "main.tf"
}

resource "azurerm_storage_blob" "mediafiles" {
  name                   = "mediafiles"
  storage_account_name   = azurerm_storage_account.companymediastorage2025.name
  storage_container_name = azurerm_storage_container.chatcontainer.name
  type                   = "Block"
  source                 = "provider.tf"
}