using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
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

var _dbContext = new MkarpovDe2024Context();


app.MapGet("/usersshifts/", async () =>
{
    using (var db = new MkarpovDe2024Context())
    {
        return Results.Ok(await db.Userlists
            .ToListAsync());
    }
})
    .AllowAnonymous()
    .WithName("UserShifts");


app.MapPost("/usersshifts/{shiftId}/{userId}", async (int shiftId, int userId) =>
    {
        using (var db = new MkarpovDe2024Context())
        {
            /*var userList = await request.ReadFromJsonAsync<Userlist>();*/
            
            var existingUserShift = await db.Userlists
                .FirstOrDefaultAsync(us => us.Shiftid == shiftId && us.Userid == userId);

            if (existingUserShift != null)
                return Results.Conflict("Пользователь с таким ID уже добавлен в эту смену.");
            
            var userShift = new Userlist()
            {
                Shiftid = shiftId,
                Userid = userId
            };
            
            db.Add(userShift);
            await db.SaveChangesAsync();
            
            return Results.Ok();
        }
    })
    .AllowAnonymous()
    .WithName("PostUserShift");


app.MapGet("/shifts", async () =>
    {
        using (var db = new MkarpovDe2024Context())
        {
            var shifts = await db.Shifts
                .ToListAsync();
            return Results.Ok(shifts);
        }
    })
    .AllowAnonymous()
    .WithName("GetShifts");


app.MapGet("/shifts/{id}", async (int id) =>
    {
        using (var db = new MkarpovDe2024Context())
        {
            var shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return Results.NotFound();
            }

            var users = await db.Userlists
                .Where(ul => ul.Shiftid == id)
                .Select(ul => ul.User)
                .ToListAsync();

            return Results.Ok(users);  
        }
    })
    .AllowAnonymous()
    .WithName("GetShiftUsers");


app.MapPost("/shifts/", async (HttpRequest request) =>
{
    using (var db = new MkarpovDe2024Context())
    {
        var shift = await request.ReadFromJsonAsync<Shift>();
        
        if (shift == null)
            return Results.BadRequest();

        db.Add(shift);
        db.SaveChanges();
        
        return Results.Created($"/orders/{shift.Shiftid}", shift);
    }
})
    .AllowAnonymous()
    .WithName("PostShift");


app.MapGet("/orders", () =>
    {
        using (var db = new MkarpovDe2024Context())
        {
            var orders = db.Orders.ToList();
            return Results.Ok(orders);
        }
    })
    .AllowAnonymous()
    .WithName("GetOrders");

app.MapPost("/orders/", async (HttpRequest request) =>
    {
        using (var db = new MkarpovDe2024Context())
        {
            var order = await request.ReadFromJsonAsync<Order>();
        
            if (order == null)
                return Results.BadRequest();
        
            db.Add(order);
            db.SaveChanges();
        
            return Results.Created($"/orders/{order.Orderid}", order);
        }
    })
    .AllowAnonymous()
    .WithName("PostOrder");

app.MapPut("/orders/", async (HttpRequest request) =>
{
    using (var db = new MkarpovDe2024Context())
    {
        var order = await request.ReadFromJsonAsync<Order>();
        
        if (order == null)
            return Results.BadRequest();

        db.Update(order);
        db.SaveChanges();
        
        return Results.Created($"/orders/{order.Orderid}", order);
    }
})
.AllowAnonymous()
.WithName("UpdateOrder");


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


app.MapGet("/users/{login}/{password}", async (string login, string password) =>
    {
        return await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Login == login && u.Password == password);
    })
    .WithName("GetUser");


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

app.MapPut("/users/", async (HttpRequest request) =>
{
    using (var db = new MkarpovDe2024Context())
    {
        var user = await request.ReadFromJsonAsync<User>();
        
        if(user == null)
            return Results.BadRequest();
        
        db.Update(user);
        db.SaveChanges();
        
        return Results.Created($"/users/{user.Userid}", user);
    }
})
    .AllowAnonymous()
    .WithName("UpdateUser");

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