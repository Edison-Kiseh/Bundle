variable "name" {
  description = "Name of the diagnostic setting"
}

variable "target_resource_id" {
  description = "The ID of the resource to enable diagnostic settings for"
}

variable "workspace_id" {
  description = "The ID of the Log Analytics Workspace to send diagnostics data to"
}