variable "acs_name" {
  description = "The name of the Azure Communication Service"
  type        = string
}


variable "resource_group_name" {
  description = "The name of the resource group in which the resources will be created"
  type        = string
}

variable "data_location" {
  description = "The location of the data"
  type        = string
  default     = "Europe"

}