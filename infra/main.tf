resource "azurerm_resource_group" "internship" {
  name     = var.resource_group_name
  location = var.location
}



module "acs" {
  source              = "./modules/acs"
  acs_name            = "acs-ASH-prod"
  resource_group_name = azurerm_resource_group.internship.name
}
module "random_id" {
  source = "./modules/random_id"
}

module "diagnostic_settings"{
source = "./modules/monitor/diagnostic_settings"
name = "diag-ASH-prod"
target_resource_id = module.acs.acs_id
workspace_id = module.law.workspace_id
}

module "web_app" {
  source              = "./modules/app_service"
  app_name            = "app-ASH-backend-prod"
  resource_group_name = azurerm_resource_group.internship.name
  app_insights_id     = module.ash_app_insights.app_insights_id
  connection_string   = module.ash_app_insights.connection_string

}

module "service_plan" {
  source              = "./modules/app_service"
  resource_group_name = azurerm_resource_group.internship.name
  connection_string   = module.ash_app_insights.connection_string

}
module "chat_frontend" {
  source              = "./modules/static_web_app"
  app_name            = "stapp-ASH-frontend-prod"
  resource_group_name = azurerm_resource_group.internship.name
  app_insights_id     = module.ash_app_insights.app_insights_id
  connection_string   = module.ash_app_insights.connection_string

}


module "ash_app_insights" {
  source              = "./modules/monitor"
  resource_group_name = azurerm_resource_group.internship.name
  location            = azurerm_resource_group.internship.location
  app_insights_name   = "appi-ASH-prod"
  workspace_id = module.law.workspace_id
  law_name     = "ash_law"
}

module "key_vault" {
  source              = "./modules/key_vault"
  location = "West Europe"
  key_vault_name      = "kv-ASH-prod-vault"
  resource_group_name = azurerm_resource_group.internship.name
}

module "key_vault_secret" {
  source              = "./modules/key_vault/secret"
  key_vault_id        = module.key_vault.key_vault_id
  secret_name         = "kv-ASH-prod-secret"
  value               = module.chat_frontend.api_key
}

module "law" {
  resource_group_name = azurerm_resource_group.internship.name
  location            = azurerm_resource_group.internship.location
  law_name            = "log-ASH-prod"
  source              = "./modules/monitor/law"
}