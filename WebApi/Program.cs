using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("/shifts", () =>
    {
        using (var db = new MkarpovDe2024Context())
        {
            var shifts = db.Users.ToList();
            return Results.Ok(shifts);
        }
    })
    .AllowAnonymous()
    .WithName("GetShifts");

app.MapGet("/users", () =>
{
    using (var db = new MkarpovDe2024Context())
    {
        var users = db.Users
            /*.Include(u => u.Userrole)*/
            .Select(u => new
            {
                u.Userid,
                u.Login,
                u.Password,
                u.Status,
                u.Lastname,
                u.Firstname,
                u.Middlename,
                u.Userroleid,
                u.Orderuserlists,
                u.Userlists,
                u.Userrole.Namerole
            })
            .ToList();
        return Results.Ok(users);
    }
})
.AllowAnonymous()
.WithName("GetUsers");

app.MapPost("/users/", async (HttpRequest request) =>
{
    using (var db = new MkarpovDe2024Context())
    {
        var user = await request.ReadFromJsonAsync<User>();
        
        if (user == null)
            return Results.BadRequest();
        
        db.Add(user);
        db.SaveChanges();
        
        return Results.Created($"/users/{user.Userid}", user);
    }
})
    .AllowAnonymous()
    .WithName("AddUser");

app.MapGet("/roles/", () =>
{
    using MkarpovDe2024Context db = new MkarpovDe2024Context();
    {
        var roles = db.Userroles
            .Select(r => new 
            {
                r.Userroleid,
                r.Namerole
            })
            .ToList();
        return Results.Ok(roles); 
    }
    
})
    .AllowAnonymous()
    .WithName("GetRoles");

/*app.MapPost("/users/", (User user) =>
{
    using (var db = new MkarpovDe2024Context())
    {
        if (user == null)
        {
            return Results.BadRequest();
        }
        
        db.Add(user);
        db.SaveChanges();
        
        return Results.Created($"/users/{user.Userid}", user);
    }
}).AllowAnonymous().WithName("AddUsers");*/

app.Run();