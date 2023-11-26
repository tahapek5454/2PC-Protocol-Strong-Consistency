var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();


app.MapGet("/ready", () =>
{

    Console.WriteLine("Order service is ready");

    return true;

});

app.MapGet("/commit", () =>
{

    Console.WriteLine("Order service is commited");

    return true;

});

app.MapGet("/rollback", () =>
{
    Console.WriteLine("Order service is rollbacked");
});

app.Run();
