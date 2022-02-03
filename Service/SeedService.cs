using Queue.Models;

namespace Queue.Service
{
    public class SeedService: BackgroundService
{
    private UserManager<UserModel> _userM;
    private RoleManager<IdentityRole> _roleM;
    private readonly IServiceProvider _provider;

    public SeedService(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new []{ "superadmin", "user" };
        foreach(var role in roles)
        {
            if(!await _roleM.RoleExistsAsync(role))
            {
                await _roleM.CreateAsync(new IdentityRole(role));
            }
        }

        if((await _userM.FindByEmailAsync("superadmin@ilmhub.uz")) == null)
        {
            var user = new UserModel()
            {
                Email = "superadmin@ilmhub.uz",
                PhoneNumber = "998950136172",
                UserName = "superadmin",
                Fullname = "Supar Admin",
                Birthdate = DateTimeOffset.Now
            };

            var result = await _userM.CreateAsync(user, "123456");
            if(result.Succeeded)
            {
                var newUser = await _userM.FindByEmailAsync("superadmin@ilmhub.uz");
                await _userM.AddToRoleAsync(newUser, "superadmin");
            }
        }

    }
}
}