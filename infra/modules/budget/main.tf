data "azurerm_subscription" "current" {}

resource "azurerm_consumption_budget_subscription" "internship" {
  subscription_id = data.azurerm_subscription.current.id
  name            = "internship2025"
  amount          = 50
  time_grain      = "Monthly"

  time_period {
    start_date = "2025-02-01T00:00:00Z"
    end_date   = "2025-06-05T00:00:00Z"
  }
  notification {
    enabled        = true
    threshold      = 80
    operator       = "GreaterThan"
    threshold_type = "Forecasted"
    contact_emails = ["norbert.berenyi@avanade.com"]
  }
}