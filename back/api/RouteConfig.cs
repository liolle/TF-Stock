public static class RouteConfig
{
    public static void RegisterRoutes(WebApplication app){
        app.MapControllerRoute(
            name: "add-product",
            pattern: "{controller=Product}/{action=Add}/{id?}"
        );

        app.MapControllerRoute(
            name: "update-product",
            pattern: "{controller=Product}/{action=Update}/{id?}"
        );

        app.MapControllerRoute(
            name: "consume-product",
            pattern: "{controller=Product}/{action=Consume}/{id?}"
        );


    }
}