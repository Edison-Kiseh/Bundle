resource "random_id" "rid" {
  byte_length = 4
}
output "dec" {
  value = random_id.rid.dec
}