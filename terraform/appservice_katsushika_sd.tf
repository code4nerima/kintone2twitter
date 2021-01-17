resource "azurerm_app_service" "app_service_katsushika_sd" {
  name                = format("%s-kintone2twitter-appservice", "katsushika-sd")
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
  https_only          = true
  logs {
        application_logs {
            file_system_level = "Information"
        }
  }
}