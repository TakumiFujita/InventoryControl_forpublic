Imports System.Web.Optimization
Imports InventoryControl.Models
Imports System.Data.Entity

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        Database.SetInitializer(Of InventoryControlContext)(New Initializer())
    End Sub
End Class
