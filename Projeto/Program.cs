using Microsoft.AspNetCore.Authentication.Cookies;
using Projeto.Data;
using Projeto.Services;
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // FORÇAR O SERVIDOR A ACEITAR ACESSO DE OUTROS DISPOSITIVOS
        builder.WebHost.UseUrls("http://0.0.0.0:5000");

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // ----------------------------------------------------
        // 1. INJEÇÃO DE DEPENDÊNCIA (DI)
        builder.Services.AddSingleton<RepositorioUsuariosJson>();
        builder.Services.AddSingleton<RepositorioTurmasJson>();
        builder.Services.AddScoped<ServicoAutenticacao>();

        // ----------------------------------------------------
        // 2. CONFIGURAÇÃO DO ESQUEMA DE AUTENTICAÇÃO
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/HelloWorld/Login";
            options.Cookie.Name = "LoginCookie";
            options.AccessDeniedPath = "/Home/Index";

            // EXPIRAÇÃO DA SESSÃO
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

            // Renova a sessão se o usuário estiver ativo
            options.SlidingExpiration = true;

            // Segurança básica para o cookie
            options.Cookie.HttpOnly = true;
        });

        builder.Services.AddSingleton<EmailService>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>().GetSection("EmailConfig");

            return new EmailService(
                config["Host"],
                int.Parse(config["Port"]),
                config["Email"],
                config["Senha"],
                config["Email"]
            );
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

      //  app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Rotas
        app.MapControllerRoute(
           name: "default",
           pattern: "{controller=HelloWorld}/{action=Login}/{id?}");

        app.Run();
    }
}
