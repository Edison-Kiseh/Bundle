variable "resource_group_name" {
  description = "The name of the resource group in which the resources will be created"
  type        = string
}

variable "location" {
  description = "The location of the resource group in which the resources will be created"
  type        = string

}

variable "azurerm_linux_web_app" {
  description = "The name of the web app"
  type        = string
  default     = "avachatbackend-ash"

}