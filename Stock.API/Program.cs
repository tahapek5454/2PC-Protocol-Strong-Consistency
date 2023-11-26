var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();


app.MapGet("/ready", () =>
{

    Console.WriteLine("Stock service is ready");

    return true;

});

app.MapGet("/commit", () =>
{

    Console.WriteLine("Stock service is commited");

    return true;

});

app.MapGet("/rollback", () =>
{
    Console.WriteLine("Stock service is rollbacked");
});

app.Run();
