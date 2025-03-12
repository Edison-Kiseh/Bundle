resource "azurerm_log_analytics_workspace" "law" {
  name                = var.law_name
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = "PerGB2018"
  daily_quota_gb = "0.024"
}

output "workspace_id" {
  value = azurerm_log_analytics_workspace.law.id
}
