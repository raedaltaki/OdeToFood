using Microsoft.EntityFrameworkCore;
using OdeToFood.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddControllers();

builder.Services.AddDbContextPool<OdeToFoodDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OdeToFoodDb"));
});

builder.Services.AddScoped<IRestaurantData, SqlRestaurantData>();

//builder.Services.AddSingleton<IRestaurantData, InMemoryRestaurantData>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.Use(SayHelloMiddlware);

async Task SayHelloMiddlware(HttpContext arg1, Func<Task> arg2)
{
    if (arg1.Request.Path.StartsWithSegments("/hello"))
        await arg1.Response.WriteAsync("Hello, World!");
    else
        await arg2();
    return;
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseNodeModules();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(e =>
{
    e.MapRazorPages();
    e.MapControllers();
});

app.MapRazorPages();

app.Run();