using GCG.Web.Booking.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MudBlazor.Services;
using MudExtensions.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

//Culture info
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var authSection = builder.Configuration.GetSection("Authentication:Schemes");

// AuthN: Cookies + OIDC (Keycloak)
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = authSection.GetSection("Cookies").GetValue<string>("LoginPath") ?? "/login"; ;
        options.LogoutPath = authSection.GetSection("Cookies").GetValue<string>("LogoutPath") ?? "/logout";
    })
    .AddOpenIdConnect("OpenIdConnect", options =>
    {
        var oidc = authSection.GetSection("OpenIdConnect");
        options.Authority = oidc.GetValue<string>("Authority");
        options.ClientId = oidc.GetValue<string>("ClientId");
        options.ClientSecret = oidc.GetValue<string>("ClientSecret");
        options.ResponseType = oidc.GetValue<string>("ResponseType") ?? "code";
        options.UsePkce = oidc.GetValue<bool?>("UsePkce") ?? true;

        options.CallbackPath = oidc.GetValue<string>("CallbackPath") ?? "/signin-oidc";
        options.SignedOutCallbackPath = oidc.GetValue<string>("SignedOutCallbackPath") ?? "/signout-callback-oidc";
        options.SaveTokens = oidc.GetValue<bool?>("SaveTokens") ?? true;
        options.GetClaimsFromUserInfoEndpoint = oidc.GetValue<bool?>("GetClaimsFromUserInfoEndpoint") ?? true;
        options.RequireHttpsMetadata = oidc.GetValue<bool?>("RequireHttpsMetadata") ?? true;

        // Scopes básicos
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });

// AuthZ (políticas simples por ahora)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles(); // (si sirves estáticos)
app.UseAntiforgery();

//Habilita seguridad antes del mapeo de componentes/endpoints
app.UseAuthentication();
app.UseAuthorization();

//Endpoints de conveniencia para login / logout
app.MapGet("/login", async (HttpContext ctx) =>
{
    // redirige al OIDC challenge y vuelve a la página actual o a /booking
    var returnUrl = ctx.Request.Query["returnUrl"].FirstOrDefault() ?? "/payments";
    await ctx.ChallengeAsync("OpenIdConnect", new AuthenticationProperties { RedirectUri = returnUrl });
}).AllowAnonymous();

app.MapGet("/logout", async (HttpContext ctx) =>
{
    // cierra sesión en cookie + OIDC (back-channel/front-channel según Keycloak)
    var returnUrl = "/booking";
    await ctx.SignOutAsync("Cookies");
    await ctx.SignOutAsync("OpenIdConnect", new AuthenticationProperties { RedirectUri = returnUrl });
}).AllowAnonymous();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
