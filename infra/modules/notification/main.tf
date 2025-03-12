resource "azurerm_resource_group" "internship" {
  name     = "internship-resources"
  location = "West Europe"
}

resource "azurerm_notification_hub" "internship" {
  name                = "internship2025"
  resource_group_name = azurerm_resource_group.internship.name
  namespace_name      = azurerm_notification_hub_namespace.internship.name
  location            = azurerm_resource_group.internship.location

  depends_on = [azurerm_notification_hub_namespace.internship]
}

resource "azurerm_notification_hub_namespace" "internship" {
  namespace_type      = "NotificationHub"
  name                = "internship2025"
  resource_group_name = azurerm_resource_group.internship.name
  location            = azurerm_resource_group.internship.location
  sku_name            = "Free"
}