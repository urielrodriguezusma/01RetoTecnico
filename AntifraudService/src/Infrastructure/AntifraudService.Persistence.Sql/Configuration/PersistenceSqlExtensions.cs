using AntifraudService.Application.Transactions.Repositories;
using AntifraudService.Persistence.Sql;
using AntifraudService.Persistence.Sql.Transactions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AntifraudService.PersistenceSql.Configuration;
public static class PersistenceSqlExtensions
{
    public static void AddPersistenceSql(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AntifraudDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BankSqlConnection"));
        });

        services.AddScoped<ITransactionReadModelRepository, TransactionRepository>();
    }
}
