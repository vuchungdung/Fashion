namespace Fashion.Migrations
{
    using Fashion.Library;
    using Fashion.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Fashion.Models.FSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Fashion.Models.FSDbContext context)
        {
            context.Users.AddOrUpdate(e => e.Id, new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@gmail.com",
                Name = "Quản trị A",
                Status = true,
                CreatedDate = DateTime.Parse("2019-09-09"),
                Image = "default-avatar.jpg",
                Password = XString.ToMD5("admin123"),
                Phone = "0987777777"
            });
        }
    }
}
