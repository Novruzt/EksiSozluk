using Bogus;
using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;




namespace EksiSozluk.Infrastructure.Persistence.Context
{
    public class DataSeed
    {
        private static List<User> GetUsers()
        {
            List<User> result = new Faker<User>("az")
                .RuleFor(u=>u.Id, f=>Guid.NewGuid())
                .RuleFor(u=>u.CreateDate, f=>f.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(u=>u.FirstName, f=>f.Person.FirstName)
                .RuleFor(u=>u.LastName, f=>f.Person.LastName)
                .RuleFor(u=>u.EmailAddress, (f, j) => f.Internet.Email(j.FirstName, j.LastName))
                .RuleFor(u=>u.UserName, (f,j)=>f.Internet.UserName(j.FirstName, j.LastName))
                .RuleFor(u=>u.Password, f=>PasswordEncryptor.Encrypt(f.Internet.Password()))
                .RuleFor(u=>u.EmailConfirmed, f=>f.PickRandom(true, false))
                .Generate(500);

            return result;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            DbContextOptionsBuilder dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(configuration["ConnectionString"]);

            DataContext context = new DataContext(dbContextBuilder.Options);

            if (context.Users.Any())
            {
                await Task.CompletedTask;
                return;
            }

            List<User> users = GetUsers();
            List<Guid> userIds =users.Select(u=>u.Id).ToList();

            await context.AddRangeAsync(users);


            var guids = Enumerable.Range(0, 150).Select(i=>Guid.NewGuid()).ToList();
            int count = 0;

            List<Entry> entries = new Faker<Entry>("az")
                .RuleFor(e=>e.Id, f=>guids[count++])
                .RuleFor(e => e.CreateDate, f => f.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(e=>e.Subject, f=>f.Lorem.Sentence(5,5))
                .RuleFor(e=>e.Content, f=>f.Lorem.Paragraph(2))
                .RuleFor(e=>e.CreatedById, f=>f.PickRandom(userIds))
                .Generate(150);

            await context.AddRangeAsync(entries);


            List<EntryComment> comments = new Faker<EntryComment>("az")
                .RuleFor(e => e.Id, f=>Guid.NewGuid())
                .RuleFor(e => e.CreateDate, f => f.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(e=>e.Content, f=>f.Lorem.Paragraph(2))
                .RuleFor(e=>e.CreatedById, f=>f.PickRandom(userIds))
                .RuleFor(e=>e.EntryId, f=>f.PickRandom(guids))
                .Generate(1000);

            await context.AddRangeAsync(comments);
            await context.SaveChangesAsync();
        }
    }
}
