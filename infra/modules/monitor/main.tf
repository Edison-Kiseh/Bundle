resource "azurerm_monitor_action_group" "budget_action_group" {
  name                = "budget_action_group"
  resource_group_name = var.resource_group_name
  short_name          = "budget_ag"
  enabled             = true
  email_receiver {
    name          = "Norbert Berenyi"
    email_address = "norbert.berenyi@avanade.com"
  }
}

output "action_group_id" {
  value = azurerm_monitor_action_group.budget_action_group.id

}

resource "azurerm_application_insights" "app_insights" {
  name                = var.app_insights_name
  resource_group_name = var.resource_group_name
  location            = var.location
  workspace_id        = var.workspace_id
  application_type    = "web"
}

resource "azurerm_application_insights_smart_detection_rule" "smart_detection_rule" {
  name                    = "Slow server response time"
  application_insights_id = azurerm_application_insights.app_insights.id
  enabled                 = false
}

output "connection_string" {
  value = azurerm_application_insights.app_insights.connection_string
}

output "app_insights_id" {
  value = azurerm_application_insights.app_insights.id

}