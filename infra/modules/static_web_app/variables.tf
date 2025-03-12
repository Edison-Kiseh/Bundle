variable "resource_group_name" {
  description = "The name of the resource group in which the resources will be created"
  type        = string

}

variable "location" {
  description = "The location of the resource group in which the resources will be created"
  type        = string
  default     = "West Europe"

}

variable "app_name" {
  description = "The name of the web app"
  type        = string
  default     = "ash-frontend"

}

variable "app_insights_id" {
  description = "The id of the application insights"
  type        = string
}


variable "connection_string" {  
  description = "The connection string of the application insights"
  type        = string
  
}