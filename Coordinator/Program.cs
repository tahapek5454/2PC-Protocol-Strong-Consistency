using Coordinator.Models.Contexts;
using Coordinator.Services.Abstraction;
using Coordinator.Services.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TwoPhaseCommitContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});



builder.Services.AddHttpClient("Order.API", client => client.BaseAddress = new("https://localhost:7299/"));
builder.Services.AddHttpClient("Stock.API", client => client.BaseAddress = new("https://localhost:7068/"));
builder.Services.AddHttpClient("Payment.API", client => client.BaseAddress = new("https://localhost:7144/"));

builder.Services.AddTransient<ITransactionService, TransactionService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/create-order-transaction", async (ITransactionService transactionService) =>
{
    //phase 1 - prepare
    var transactionId = await transactionService.CreateTransactionAsync();
    await transactionService.PrepareServicesAsync(transactionId);
    bool transactionState = await transactionService.CheckReadyServicesAsync(transactionId);

    if (transactionState)
    {
        //phase 2 - commit
        await transactionService.CommitAsync(transactionId);
        transactionState = await transactionService.CheckTransactionStateServicesAsync(transactionId);

        if(!transactionState)
        {
            await transactionService.RollbackAsync(transactionId);
        }
    }
});

app.Run();
