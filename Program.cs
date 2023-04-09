using ChatServer.Hubs;
using ChatServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddSingleton<ManageUserInfo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4300")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapRazorPages();

app.UseEndpoints(routes =>
{
    routes.MapControllerRoute("default",
                      "{controller}/{action}");
    routes.MapHub<ChatHub>("/chatHub");
});

app.Run();
