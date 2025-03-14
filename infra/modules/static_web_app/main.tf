resource "azurerm_static_web_app" "chat_frontend" {
  name                = var.app_name
  resource_group_name = var.resource_group_name
  location            = var.location
  sku_tier            = "Free"
}
output "api_key" {
  value = azurerm_static_web_app.chat_frontend.api_key

}