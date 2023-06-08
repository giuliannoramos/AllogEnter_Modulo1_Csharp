using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Univali.Api;
using Univali.Api.Configuration;
using Univali.Api.DbContexts;
using Univali.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configurando porta para ser 5000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
});

// Add services to the container.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<Data>();

builder.Services.AddDbContext<CustomerContext>(options =>
{
    options.UseNpgsql("Host=localhost;Database=Univali;Username=postgres;Password=1973");
});

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
}).ConfigureApiBehaviorOptions(setupAction =>
       {
           setupAction.InvalidModelStateResponseFactory = context =>
           {
               // Cria a fábrica de um objeto de detalhes de problema de validação
               var problemDetailsFactory = context.HttpContext.RequestServices
                   .GetRequiredService<ProblemDetailsFactory>();


               // Cria um objeto de detalhes de problema de validação
               var validationProblemDetails = problemDetailsFactory
                   .CreateValidationProblemDetails(
                       context.HttpContext,
                       context.ModelState);


               // Adiciona informações adicionais não adicionadas por padrão
               validationProblemDetails.Detail =
                   "See the errors field for details.";
               validationProblemDetails.Instance =
                   context.HttpContext.Request.Path;


               // Relata respostas do estado de modelo inválido como problemas de validação
               validationProblemDetails.Type =
                   "https://courseunivali.com/modelvalidationproblem";
               validationProblemDetails.Status =
                   StatusCodes.Status422UnprocessableEntity;
               validationProblemDetails.Title =
                   "One or more validation errors occurred.";


               return new UnprocessableEntityObjectResult(
                   validationProblemDetails)
               {
                   ContentTypes = { "application/problem+json" }
               };
           };
       });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.ResetDatabaseAsync();

app.Run();

/* CRIANDO ESTE PROJETO PELO TERMINAL
Windows PowerShell
Copyright (C) Microsoft Corporation. Todos os direitos reservados.

Experimente a nova plataforma cruzada PowerShell https://aka.ms/pscore6

PS C:\Users\6444520> ls


    Diretório: C:\Users\6444520


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        05/12/2022     14:45                .android
d-----        12/12/2022     12:25                .cache
d-----        07/02/2018     12:51                .cordova
d-----        12/12/2022     12:27                .dbus-keyrings
d-----        07/02/2018     12:51                .dotnet
d-----        09/11/2022     12:05                .ms-ad
d-----        07/02/2018     12:51                .nbi
d-----        24/05/2023     14:05                .omnisharp
d-----        12/12/2022     12:23                .VirtualBox
d-----        23/11/2022     09:17                .vscode
d-r---        24/05/2023     14:00                3D Objects
d-----        05/12/2022     11:00                Assets
d-r---        24/05/2023     14:00                Contacts
d-r---        24/05/2023     14:00                Desktop
d-r---        24/05/2023     14:00                Documents
d-r---        24/05/2023     14:00                Downloads
d-r---        24/05/2023     14:00                Favorites
d-----        05/12/2022     11:00                Library
d-r---        24/05/2023     14:00                Links
d-r---        24/05/2023     14:00                Music
d-r---        09/11/2022     10:23                OneDrive
d-r---        24/05/2023     14:00                Pictures
d-----        05/12/2022     11:00                ProjectSettings
d-r---        24/05/2023     14:00                Saved Games
d-r---        24/05/2023     14:00                Searches
d-----        23/11/2022     15:27                source
d-----        05/12/2022     11:00                Temp
d-r---        24/05/2023     14:00                Videos
-a----        24/05/2023     14:00           1818 dotnet.txt
-a----        07/02/2018     15:07         108485 local.txt
-a----        07/02/2018     15:07           7508 locallow.txt
-a----        24/05/2023     14:00           1588 nbi.txt
-a----        25/08/2017     10:02         157689 package-lock.json
-a----        25/08/2017     10:02            471 semantic.json


PS C:\Users\6444520> cd source
PS C:\Users\6444520\source> cd repos


    Diretório: C:\Users\6444520\source\repos


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        24/05/2023     14:04                AllogEnter_Modulo1_Csharp




    Diretório: C:\Users\6444520\source\repos


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        24/05/2023     14:22                Univali


PS C:\Users\6444520\source\repos> ls
    Diretório: C:\Users\6444520\source\repos


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        24/05/2023     14:04                AllogEnter_Modulo1_Csharp
d-----        24/05/2023     14:22                Univali


PS C:\Users\6444520\source\repos> cd Univali
PS C:\Users\6444520\source\repos\Univali> dotnet new sln --name Univali

Bem-vindo(a) ao .NET 7.0.
---------------------
Versão do SDK: 7.0.100

Telemetria
---------
As ferramentas do .NET coletam dados de uso para ajudar-nos a aprimorar sua experiência. Eles são coletados pela Microsoft e compartilhados com a comunidade. Você pode recusar a telemetria definindo a variável de ambiente DOTNET_CLI_TELEMETRY_OPTOUT como '1' ou 'true' usando seu shell favorito.

Leia mais sobre a telemetria das Ferramentas da CLI do .NET: https://aka.ms/dotnet-cli-telemetry

----------------
Um certificado de desenvolvimento HTTPS ASP.NET Core foi instalado.
Saiba mais sobre HTTPS: https://aka.ms/dotnet-https
----------------
Escreva seu primeiro aplicativo: https://aka.ms/dotnet-hello-world
Descubra o que há de novo: https://aka.ms/dotnet-whats-new
Explore a documentação: https://aka.ms/dotnet-docs
Relate problemas e encontre a fonte no GitHub: https://github.com/dotnet/core
Use 'dotnet --help' para ver os comandos disponíveis ou visite: https://aka.ms/dotnet-cli
--------------------------------------------------------------------------------------
O modelo "Arquivo de Solução" foi criado com êxito.


    Diretório: C:\Users\6444520\source\repos\Univali


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        24/05/2023     14:23                src


PS C:\Users\6444520\source\repos\Univali\src> dotnet new webapi -n Univali.Api
O modelo "API Web do ASP.NET Core" foi criado com êxito.

Processando ações pós-criação...
Restaurando C:\Users\6444520\source\repos\Univali\src\Univali.Api\Univali.Api.csproj:
  Determinando os projetos a serem restaurados...
  C:\Users\6444520\source\repos\Univali\src\Univali.Api\Univali.Api.csproj restaurado (em 2,01 sec).
A restauração foi bem-sucedida.



    Diretório: C:\Users\6444520\source\repos\Univali\src


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        24/05/2023     14:24                Univali.Api


PS C:\Users\6444520\source\repos\Univali\src> cd ..
PS C:\Users\6444520\source\repos\Univali> ls



Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        24/05/2023     14:24                src
-a----        24/05/2023     14:23            441 Univali.sln


PS C:\Users\6444520\source\repos\Univali> dotnet sln Univali add src/Univali.Api/Univali.Api.csproj
Não foi possível encontrar a solução ou o diretório 'Univali'.
PS C:\Users\6444520\source\repos\Univali> dotnet sln Univali.sln  add src/Univali.Api/Univali.Api.csproj
O projeto 'src\Univali.Api\Univali.Api.csproj' foi adicionado à solução.
PS C:\Users\6444520\source\repos\Univali> */