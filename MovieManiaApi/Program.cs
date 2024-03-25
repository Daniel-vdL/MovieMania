using Microsoft.EntityFrameworkCore;
using MovieManiaApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

using var db = new AppDbcontext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbcontext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
