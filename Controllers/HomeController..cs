using Queue.Models;
namespace Queue.Controllers;
public partial class HomeController : Controller
{
    
    [HttpGet]
    public IActionResult TakeQueue()
    {
        return View();
    }
    [HttpPost]
    public IActionResult TakeQueue([FromForm] QueueModel model)
    {
 
        
        var lastuser=_dbcontext.Queues.OrderBy(p => p.CreatedAt).LastOrDefault();
        
        var user=new QueueModel();
        user.ID=model.ID;
        user.CustomerName=model.CustomerName;
        user.CreatedAt=model.CreatedAt=DateTimeOffset.UtcNow.ToLocalTime();
        user.Phone=model.Phone;
        if(lastuser==null){

            user.ServiceEndTime=DateTimeOffset.UtcNow.ToLocalTime();
            user.ExpirationTime=user.ServiceEndTime.AddMinutes(15);
        }
        if(lastuser == null || lastuser.ExpirationTime < DateTimeOffset.UtcNow.ToLocalTime()||lastuser.IsActive==false)
        {
            user.ServiceEndTime=DateTimeOffset.UtcNow.ToLocalTime();
            user.ExpirationTime=user.ServiceEndTime.AddMinutes(15);
            user.IsActive=model.IsActive=true;
            
        }
        else
        {
            user.ServiceEndTime=lastuser.ServiceEndTime.AddMinutes(15);
            user.ExpirationTime=user.ServiceEndTime;
            user.IsActive=model.IsActive=true;
        }
    
    
        try
        {
            _dbcontext.Queues.Add(user);
            _dbcontext.SaveChanges();
        }
        catch(Exception e)
        {
            return View(e.Message);
        }
        return RedirectToAction("ShowQueue",user);
        

    }

    [HttpGet]
    public IActionResult Login(string returnUrl) 
        => View(new LoginModel() { ReturnUrl = returnUrl ?? string.Empty });

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);
        if(user == default)
        {
            ModelState.AddModelError("Password", "Email yoki parol noto'g'ri kiritilgan.");
            return View(model);
        }

        var result = await _signinManager.PasswordSignInAsync(user, model.Password, false, false);
        if(result.Succeeded)
        {
            return LocalRedirect(model.ReturnUrl ?? "/");
        }

        return BadRequest(result.IsNotAllowed);
    }

    
    [HttpGet]
    public IActionResult ShowQueue([FromRoute]QueueModel model)
    {
        var client =_dbcontext.Queues.FirstOrDefault(u=>u.ID==model.ID);
        return View(client);
    }

}