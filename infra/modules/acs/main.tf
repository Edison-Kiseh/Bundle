resource "azurerm_communication_service" "acs" {
  name                = var.acs_name
  resource_group_name = var.resource_group_name
  data_location       = var.data_location
}

output "acs_id" {
  value = azurerm_communication_service.acs.id
}