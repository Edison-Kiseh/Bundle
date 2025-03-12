resource "azurerm_key_vault_secret" "key_vault_secret" {
  key_vault_id = var.key_vault_id
  name         = var.secret_name
  value        = var.value
}
