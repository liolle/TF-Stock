public static class RouteConfig
{
    public static void RegisterRoutes(WebApplication app){
        app.MapControllerRoute(
            name: "add-product",
            pattern: "{controller=Product}/{action=Add}/{id?}"
        );

    }
}