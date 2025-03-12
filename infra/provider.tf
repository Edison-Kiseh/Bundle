provider "azurerm" {
  features {
  }
}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.19.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "management"
    storage_account_name = "terraformanagement"
    container_name       = "tfstateblob"
    key                  = "terraform.tfstate"
  }
}