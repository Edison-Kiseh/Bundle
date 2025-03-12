resource "azurerm_monitor_diagnostic_setting" "diag_set" {
  name               = var.name
  target_resource_id = var.target_resource_id
  log_analytics_workspace_id = var.workspace_id
  metric {
    category = "AllMetrics"
    enabled  = true
  }

  enabled_log {
    category = "ChatOperational"
  }
}

