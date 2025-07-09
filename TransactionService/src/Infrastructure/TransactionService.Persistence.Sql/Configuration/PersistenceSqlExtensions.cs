using AntifraudService.Persistence.Sql.Transactions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.UnitOfWork;
using TransactionService.Domain.Transaction.Repositories;

namespace TransactionService.Persistence.Sql.Configuration;
public static class PersistenceSqlExtensions
{
    public static void AddPersistenceSql(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BankSqlConnection"));
        });

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
