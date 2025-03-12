variable "key_vault_name" {
  description = "The name of the key vault"
  type        = string
}
variable "resource_group_name" {
  description = "The name of the resource group in which the resources will be created"
  type        = string

}

variable "location" {
  description = "The location of the resource group in which the resources will be created"
  type        = string
  
}