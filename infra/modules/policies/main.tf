resource "azurerm_subscription_policy_assignment" "region" {
  name                 = "region"
  subscription_id      = "3658a383-726e-4769-bc25-471aa0c93410"
  policy_definition_id = "/providers/Microsoft.Authorization/policyDefinitions/06a78e20-9358-41c9-923c-fb736d382a4d"
  description          = "Shows all virtual machines not using managed disks"
  display_name         = "Audit VMs without managed disks assignment"
}