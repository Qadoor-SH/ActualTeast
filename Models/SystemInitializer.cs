using System.Security.Claims;
using ActualTeast.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActualTeast.Models
{
    public class SystemInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        public SystemInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider=serviceProvider;
        }
        public async Task SeedRoles()
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                dbContext.Database.EnsureCreated();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if(!await roleManager.RoleExistsAsync(UserRoles.Admin
                    .ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole {
                        Name=UserRoles.Admin.ToString(),
                    });
                }
                if(!await roleManager.RoleExistsAsync(UserRoles.User
                    .ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole {
                        Name=UserRoles.User.ToString(),
                    });
                }
            }

        }
        public async Task SeedUsers()
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                dbContext.Database.EnsureCreated();
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var user = await dbContext.Users.FirstOrDefaultAsync(m => m.Email=="user0AU@gmail.com");
                if(user==null)
                {
                    var appuser = new User {
                        FirstName="user0FN",
                        LastName="user0LN",
                        Email="user0AU@gmail.com",
                        EmailConfirmed=true,
                        PhoneNumber="775175771",
                        UserName="user0AU",
                        SecurityStamp=Guid.NewGuid().ToString(),
                    };

                    var result = await _userManager.CreateAsync(appuser,"Rs_sys123");
                    if(result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appuser,UserRoles.Admin.ToString());
                        await _userManager.AddToRoleAsync(appuser,UserRoles.User.ToString());
                        var currentRoleClaim = new Claim(ClaimTypes.Role,UserRoles.Admin.ToString());
                        await _userManager.AddClaimAsync(appuser,currentRoleClaim);
                    }
                }
                user=await dbContext.Users.FirstOrDefaultAsync(m => m.Email=="user1U@gmail.com");
                if(user==null)
                {
                    var appuser = new User {
                        FirstName="user1Fn",
                        LastName="user1Ln",
                        Email="user1U@gmail.com",
                        EmailConfirmed=true,
                        PhoneNumber="775175771",
                        UserName="user1U",
                        SecurityStamp=Guid.NewGuid().ToString(),
                    };

                    var result = await _userManager.CreateAsync(appuser,"Rs_sys123");
                    if(result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appuser,UserRoles.User.ToString());
                        var currentRoleClaim = new Claim(ClaimTypes.Role,UserRoles.User.ToString());
                        await _userManager.AddClaimAsync(appuser,currentRoleClaim);
                    }
                }
                user=await dbContext.Users.FirstOrDefaultAsync(m => m.Email=="user2U@gmail.com");
                if(user==null)
                {
                    var appuser = new User {
                        FirstName="user2Fn",
                        LastName ="user2Ln",
                        Email="user2U@gmail.com",
                        EmailConfirmed=true,
                        PhoneNumber="775175771",
                        UserName="user2U",
                        SecurityStamp=Guid.NewGuid().ToString(),
                    };

                    var result = await _userManager.CreateAsync(appuser,"Rs_sys123");
                    if(result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appuser,UserRoles.User.ToString());
                        var currentRoleClaim = new Claim(ClaimTypes.Role,UserRoles.User.ToString());
                        await _userManager.AddClaimAsync(appuser,currentRoleClaim);
                    }
                }
                user=await dbContext.Users.FirstOrDefaultAsync(m => m.Email=="user3U@gmail.com");
                if(user==null)
                {
                    var appuser = new User {
                        FirstName="user3Fn",
                        LastName="user3Ln",
                        Email="user3U@gmail.com",
                        EmailConfirmed=true,
                        PhoneNumber="775175771",
                        UserName="user3U",
                        SecurityStamp=Guid.NewGuid().ToString(),
                    };

                    var result = await _userManager.CreateAsync(appuser,"Rs_sys123");
                    if(result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appuser,UserRoles.User.ToString());
                        var currentRoleClaim = new Claim(ClaimTypes.Role,UserRoles.User.ToString());
                        await _userManager.AddClaimAsync(appuser,currentRoleClaim);
                    }
                }
                user=await dbContext.Users.FirstOrDefaultAsync(m => m.Email=="user4U@gmail.com");
                if(user==null)
                {
                    var appuser = new User {
                        FirstName="user4Fn",
                        LastName ="user4Ln",
                        Email="user4U@gmail.com",
                        EmailConfirmed=true,
                        PhoneNumber="775175771",
                        UserName="user4U",
                        SecurityStamp=Guid.NewGuid().ToString(),
                    };

                    var result = await _userManager.CreateAsync(appuser,"Rs_sys123");
                    if(result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appuser,UserRoles.User.ToString());
                        var currentRoleClaim = new Claim(ClaimTypes.Role,UserRoles.User.ToString());
                        await _userManager.AddClaimAsync(appuser,currentRoleClaim);
                    }
                }
            }
        }
    }
}
