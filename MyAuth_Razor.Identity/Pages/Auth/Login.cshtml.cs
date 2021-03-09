using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAuth_Razor.Core.Entities;

namespace MyAuth_Razor.Identity.Pages.Auth
{
  public class LoginModel : PageModel
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    [BindProperty]
    public AuthUser AuthUser { get; set; }

    public LoginModel(
      UserManager<IdentityUser> userManager,
      SignInManager<IdentityUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public async Task<ActionResult> OnPostAsync()
    {
      if(ModelState.IsValid)
      {
        var user = await _userManager.FindByNameAsync(AuthUser.Email);
        if(user == null)
        {
          ModelState.AddModelError("AuthUser.Email", "Invalid login!");
          return Page();
        }

        var result = await _signInManager.PasswordSignInAsync(
          AuthUser.Email, AuthUser.Password, isPersistent: false, lockoutOnFailure: false);

        if(!result.Succeeded)
        {
          ModelState.AddModelError(string.Empty, "Invalid login!");
          return Page();
        } else
        {
          return RedirectToPage(Request.Query["ReturnUrl"]);
        }
      }
      else
      {
        return Page();
      }
    }
  }
}
