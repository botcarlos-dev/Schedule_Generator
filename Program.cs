using Microsoft.EntityFrameworkCore;
using HorariosIPBejaMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using HorariosIPBejaMVC.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner com filtro global de autoriza��o (exceto para p�ginas p�blicas)
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Configurar o ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar a autentica��o baseada em cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Defina conforme necess�rio
        options.SlidingExpiration = true;
    });

// Configurar a pol�tica de autoriza��o para "Funcion�rio" com "Gest�o"
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FuncionarioGestaoPolicy", policy =>
        policy.RequireRole("Funcion�rio").Requirements.Add(new GestaoRequirement()));
});

// Adicionar suporte para sess�o
builder.Services.AddDistributedMemoryCache(); // Necess�rio para usar sess�es
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Necess�rio para conformidade com GDPR
    options.Cookie.SameSite = SameSiteMode.Lax; // Permite o envio do cookie em solicita��es POST
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Alinha com o protocolo da solicita��o
});

// Adicionar o handler de autoriza��o
builder.Services.AddScoped<IAuthorizationHandler, GestaoRequirementHandler>();

var app = builder.Build();

// Configurar o pipeline de requisi��es HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adicionar o middleware de sess�o antes de autentica��o e autoriza��o
app.UseSession(); // Adiciona suporte para sess�es

// Adicionar o middleware de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Definir a rota padr�o para Account/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
