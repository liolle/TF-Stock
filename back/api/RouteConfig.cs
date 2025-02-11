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
            name: "get-transactions",
            pattern: "{controller=Transaction}/{action=ByProduct}/{id?}"
        );

        //User
        app.MapControllerRoute(
            name: "login",
            pattern: "{controller=User}/{action=Login}/{id?}"
        );

        app.MapControllerRoute(
            name: "logout",
            pattern: "{controller=User}/{action=Logout}/{id?}"
        );

        app.MapControllerRoute(
            name: "register",
            pattern: "{controller=User}/{action=Register}/{id?}"
        );

    }
}