using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Application.Services.Implementations;
using Tasks.Application.Services.Interfaces;
using Tasks.Infrastructure.Persistence;
using Tasks.Infrastructure.Repository.Implementations;
using Tasks.Infrastructure.Repository.Interfaces;

namespace Tasks.Application.Services.Configuration
{
    public class DIConfiguration
    {        
        private readonly IServiceCollection _services;

        public DIConfiguration(IServiceCollection services)
        {
            _services = services;            
        }

        public void ConfigureServices()
        {
            _services.AddDbContext<ToDoTaskDbContext>();                       
            _services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
            _services.AddScoped<IToDoTaskService, ToDoTaskService>();
        }        
    }
}
