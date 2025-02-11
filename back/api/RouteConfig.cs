public static class RouteConfig
{
    public static void RegisterRoutes(WebApplication app){
        //Product
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


        //Transaction
        app.MapControllerRoute(
            name: "consume-product",
            pattern: "{controller=Transaction}/{action=ByProduct}/{id?}"
        );

    }
}