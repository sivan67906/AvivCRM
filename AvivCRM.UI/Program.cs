WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiGatewayCall", client =>
{
    //client.BaseAddress = new Uri("http://localhost:6300/"); //Api Gateway
    //client.BaseAddress = new Uri("https://avivgateway.vysncapital.com/"); //ServerApi Gateway
    //client.BaseAddress = new Uri("http://localhost:6201/api/"); //Configuration WebApi
    client.BaseAddress = new Uri("https://localhost:7121/api/"); //Environment WebApi
});

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "Configuration",
//    pattern: "{area:exists}/{controller=Consumer}/{action=Consumer1}/{id?}")
//    .WithStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id:int?}")
//    .WithStaticAssets();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id:int?}")
    //pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapDefaultControllerRoute();

app.Run();