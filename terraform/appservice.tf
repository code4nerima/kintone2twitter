provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "resource_group" {
  name     = "kintone2twitter-resources"
  location = "japaneast"
}

resource "azurerm_app_service_plan" "app_service_plan" {
  name                = "kintone2twitter-asp"
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "app_service" {
  name                = "kintone2twitter-appservice"
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
  https_only          = true
/*
  site_config {
    #dotnet_framework_version = "v4.0"
    remote_debugging_enabled = true
    remote_debugging_version = "VS2019"
  }
*/
  logs {
        application_logs {
            file_system_level = "Information"
        }
  }
}