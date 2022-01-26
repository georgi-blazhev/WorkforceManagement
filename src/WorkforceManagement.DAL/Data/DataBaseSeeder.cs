using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.Data
{
    [ExcludeFromCodeCoverage]
    public class DatabaseSeeder
     {
        public static void Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                if (context.Database.EnsureCreated())
                {
                    PasswordHasher<User> hasher = new();

                    User admin = new User()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Email = "admin@test.test",
                        NormalizedEmail = "admin@test.test".ToUpper(),
                        EmailConfirmed = true,
                        UserName = "admin",
                        NormalizedUserName = "admin".ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Admin",
                        LastName = "Adminov",
                        CreatedAt = DateTime.Now
                    };

                    admin.PasswordHash = hasher.HashPassword(admin, "adminpass");

                    IdentityRole adminRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };
                    IdentityRole regularRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Regular",
                        NormalizedName = "Regular".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityUserRole<string> identityUserRole = new() { RoleId = adminRole.Id, UserId = admin.Id };

                    #region 2022 Holidays
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 01, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 03, 03)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 04, 22)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 04, 23)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 04, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 04, 25)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 05, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 05, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 05, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 09, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 09, 22)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 12, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 12, 25)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2022, 12, 26)));
                    #endregion

                    #region 2023 Holidays
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 01, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 03, 03)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 04, 14)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 04, 15)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 04, 16)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 04, 17)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 05, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 05, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 05, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 09, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 09, 22)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 12, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 12, 25)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2023, 12, 26)));
                    #endregion

                    #region 2024 Holidays
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 01, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 03, 03)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 05, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 05, 03)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 05, 04)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 05, 05)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 05, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 05, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 09, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 09, 22)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 12, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 12, 25)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2024, 12, 26)));
                    #endregion

                    #region 2025 Holidays
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 01, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 03, 03)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 04, 18)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 04, 19)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 04, 20)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 04, 21)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 05, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 05, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 05, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 09, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 09, 22)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 12, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 12, 25)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2025, 12, 26)));
                    #endregion

                    #region 2026 Holidays
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 01, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 03, 03)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 04, 10)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 04, 11)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 04, 12)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 04, 13)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 05, 01)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 05, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 05, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 09, 06)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 09, 22)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 12, 24)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 12, 25)));
                    context.OfficialHolidays.Add(new Holiday(new DateTime(2026, 12, 26)));
                    #endregion


                    context.Roles.Add(adminRole);
                    context.Roles.Add(regularRole);

                    context.Users.Add(admin);

                    context.UserRoles.Add(identityUserRole);
                    context.SaveChanges();
                }

            }
        }
    }
}
