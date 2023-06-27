using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Univali.Api;
using Univali.Api.Configuration;
using Univali.Api.DbContexts;
using Univali.Api.Extensions;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Customers.Commands.DeleteCustomer;
using Univali.Api.Features.Customers.Commands.PatchCustomer;
using Univali.Api.Features.Customers.Commands.UpdateCustomer;
using Univali.Api.Features.Customers.Queries.GetAddresses;
using Univali.Api.Features.Customers.Queries.GetAllCustomers;
using Univali.Api.Features.Customers.Queries.GetCustomerCpf;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;
using Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;
using Univali.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurando porta para ser 5000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
});

// Add services to the container.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<Data>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IGetCustomerDetailQueryHandler, GetCustomerDetailQueryHandler>();

builder.Services.AddScoped<IGetCustomerCpfQueryHandler, GetCustomerCpfQueryHandler>();

builder.Services.AddScoped<IGetAllCustomersQueryHandler, GetAllCustomersQueryHandler>();

builder.Services.AddScoped<IGetCustomerWithAddressesQueryHandler, GetCustomerWithAddressesQueryHandler>();

builder.Services.AddScoped<ICreateCustomerCommandHandler, CreateCustomerCommandHandler>();

builder.Services.AddScoped<IUpdateCustomerCommandHandler, UpdateCustomerCommandHandler>();

builder.Services.AddScoped<IDeleteCustomerCommandHandler, DeleteCustomerCommandHandler>();

builder.Services.AddScoped<IPatchCustomerCommandHandler, PatchCustomerCommandHandler>();

builder.Services.AddScoped<IGetAddressesQueryHandler, GetAddressesQueryHandler>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        // Declaramos o que deverá ser validado
        // O tempo de expiração do token é validado automaticamente.
        // Obriga a validação do emissor
        ValidateIssuer = true,
        // Obriga a validação da audiência
        ValidateAudience = true,
        // Obriga a validação da chave de assinatura`
        ValidateIssuerSigningKey = true,

        // Agora declaramos os valores das propriedades que serão validadas
        // Apenas tokens  gerados por esta api serão considerados válidos.
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        // Apenas tokens desta audiência serão considerados válidos.
        ValidAudience = builder.Configuration["Authentication:Audience"],
        // Apenas tokens com essa assinatura serão considerados válidos.
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!))
    };
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

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

var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.ResetDatabaseAsync();

app.Run();

//  Passo 1: Crie uma nova solução
// dotnet new sln --name Univali

//  Passo 2: Crie uma pasta chamada 'src'
// mkdir src

//  Passo 3: Acesse a pasta 'src'
// cd src

//  Passo 4: Crie um novo projeto do tipo webapi
// dotnet new webapi -n Univali.Api

//  Passo 5: Acesse o diretório do projeto
// cd Univali.Api

//  Passo 6: Adicione o projeto à solução
// dotnet sln Univali.sln add src/Univali.Api/Univali.Api.csproj