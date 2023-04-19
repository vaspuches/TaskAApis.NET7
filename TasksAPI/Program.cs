using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Tasks.Application.Services.Configuration;
using Tasks.Web.Api.Automapper;

var builder = WebApplication.CreateBuilder(args);

// Add DIConfiguration
DIConfiguration diConfig = new(builder.Services);
diConfig.ConfigureServices();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks API", Version = "v1" });
    c.EnableAnnotations();
});

// Automapper
builder.Services.AddAutoMapper(typeof(AutoMapping));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Novedades .NET 7 
/*Crear una api sin controladores habilitando OpenApi para visualizar swagger

Novedades .NET7
Minimal Api - personal opinion --- no sirven para empresas grandes, 1 controller y 45 endpoints en un fichero.
 
llega c# 11 - Ver detalles
SignalR - permiten Injeccion de dependencias
Mapping to JSON columns for SQL - guardas como json en la bbdd
Distributed transactions - investigar*
gRPC - investigar* - gRPC es un mecanismo moderno de invocación de métodos remotos (RPC) entre procesos/sistemas/servicios
Microsoft Orleans - investigar* - state machine fuera de la app
Custom reverse engineering templates - 
Default Caracteristics - librerias del lenguaje -- mejora en librerias
Dev-tunnels -- un "sdk" - crea un tuner en la maquina local y la conecta a un contenedor en azure.
			-- permite un debug de app de production en production, para poder replicar algun bug puntual
			--que no se pueda reproducir en dev o local.
			
Migrar a Asp Net Core desde Framework - dentro de VS permite migrar apps de forma sencilla, añade una Asp Net Core enfrente de tu app,
	-- con reverse prox, redirecciona de una a otra...todo sigue funcionando mientras tu vas migrando los endpoints de uno en uno, y no piedes disponibilidad

Ejemplo Minimal API
var toDotasks = ToDoTasks[]
{
	new ToDoTasks("Work", "2025-02-01", "Completed")
	new ToDoTasks("Train", "2025-03-01", "InProgress")
	new ToDoTasks("Sleep", "2025-04-01", "NotStarted")
}

app.MapGet("*ToDOTask/status{status}"), (int quantity){
	
	return toDotasks.Take(quantity);
}).AddEndpointFilter(async (context, next) => //next es el delegado
	int quantity = context.GetArgument<int>(0);
	
	if(quantity <= 0){
		return Results.Problem("Valor debe ser superior a 0");
	}
	
	return await next(context);
)

public record ToDoTasks(String Description, DateTime DueDate, String Status);

public class MyFilter : IEndPointFilter{
	//se pueden hacer filtros a través de interfaces para reutilizar los filtros 
}
*/
