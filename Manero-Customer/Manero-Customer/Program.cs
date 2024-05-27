using Azure.Messaging.ServiceBus;
using Blazored.SessionStorage;
using Manero_Customer.Components;
using Manero_Customer.Components.Account;
using Manero_Customer.Data;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ProductService> ();
builder.Services.AddScoped<CategoryService> ();
builder.Services.AddScoped<SubCategoryService> ();
builder.Services.AddScoped<FilterService> ();
builder.Services.AddSingleton<SharesDataService> ();
builder.Services.AddScoped<ConfirmAccountService> ();
builder.Services.AddScoped<SignInService>();
builder.Services.AddScoped<VerifyAccountService> ();
builder.Services.AddScoped<ProductDetailsService>();
builder.Services.AddScoped<CartService> ();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<UserService> ();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieService> ();



builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddSingleton<ServiceBusSender>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
    var queueName = builder.Configuration["ServiceBus:SenderQueue"];
    var client = new ServiceBusClient(connectionString);
    return client.CreateSender(queueName);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SessionIdMiddleware>();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Manero_Customer.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
