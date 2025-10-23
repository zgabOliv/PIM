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

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // ----------------------------------------------------
        // 1. INJE��O DE DEPEND�NCIA (DI)
        // O c�digo de cria��o de usu�rio TEMPOR�RIO FOI REMOVIDO DAQUI.
        // ----------------------------------------------------
        builder.Services.AddSingleton<RepositorioUsuariosJson>();
        builder.Services.AddScoped<ServicoAutenticacao>();

        // ----------------------------------------------------
        // 2. CONFIGURA��O DO ESQUEMA DE AUTENTICA��O
        // ----------------------------------------------------
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                // Define o caminho para onde o usu�rio deve ser redirecionado se tentar acessar uma �rea protegida
                options.LoginPath = "/HelloWorld/Login";
                options.Cookie.Name = "LoginCookie";
                // Garante que o usu�rio que tentar acessar uma Dashboard sem Role correta seja redirecionado
                options.AccessDeniedPath = "/Home/Index";
            });


        var app = builder.Build();

 
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        // Essencial para carregar arquivos CSS, JS e Views:
        app.UseStaticFiles();

        app.UseRouting();

       
        app.UseAuthentication();
        app.UseAuthorization();



        app.MapControllerRoute(
            name: "default",
        
            pattern: "{controller=HelloWorld}/{action=Login}/{id?}");

        app.Run();
    }
}
