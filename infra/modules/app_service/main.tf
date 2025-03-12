resource "azurerm_linux_web_app" "web_app" {
  name                = var.app_name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.service_plan.id

  site_config {
    always_on = false
  }

  app_settings = {
    ApplicationInsightsAgent_EXTENSION_VERSION = "~3"
    azurerm_application_insights               = var.app_insights_id
    APPLICATIONINSIGHTS_CONNECTION_STRING      = var.connection_string
  }
  lifecycle {
    prevent_destroy = false
  }
}

resource "azurerm_service_plan" "service_plan" {
  name                = var.service_plan_name
  location            = var.location
  resource_group_name = var.resource_group_name
  os_type             = "Windows"
  sku_name            = "B1"
  worker_count        = 2


}