variable "law_name" {
  description = "The name of the Log Analytics Workspace"
  type        = string

}

variable "workspace_id" {
  description = "The ID of the Log Analytics Workspace"
  type        = string

}
variable "diag_set_name" {
  description = "The name of the Diagnostic Setting"
  type        = string
  default     = "default"

}

variable "app_insights_name" {
  description = "The name of the Application Insights"
  type        = string
  default     = "appinsights"

}


variable "resource_group_name" {
  description = "The name of the resource group in which the resources will be created"
  type        = string

}

variable "location" {
  description = "The location of the resource group in which the resources will be created"
  type        = string

}