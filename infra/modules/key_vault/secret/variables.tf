variable "key_vault_id" {
    description = "The ID of the key vault"
    type        = string
}

variable "secret_name" {
    description = "The name of the secret"
    type    = string
  
}

variable "value" {
    description = "The value of the secret"
    type        = string
}