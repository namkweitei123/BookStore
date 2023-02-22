using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Enums;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Album.Areas.Admin.Pages.Role {

    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel {
    private readonly RoleManager<IdentityRole> _roleManager;

    public DeleteModel (RoleManager<IdentityRole> roleManager) {
      _roleManager = roleManager;
    }

    public class InputModel {
      [Required]
      public string ID { set; get; }
      public string Name { set; get; }

    }

    [BindProperty]
    public InputModel Input { set; get; }

    [BindProperty]
    public bool isConfirmed { set; get; }

    [TempData] 
    public string StatusMessage { get; set; }

    public IActionResult OnGet () => NotFound ("Not found");

    public async Task<IActionResult> OnPost () {

      if (!ModelState.IsValid) {
        return NotFound ("Can not delete");
      }

      var role = await _roleManager.FindByIdAsync (Input.ID);
      if (role == null) {
        return NotFound ("Can't see the role to delete");
      }

      ModelState.Clear ();

      if (isConfirmed) {
        //Xóa
        await _roleManager.DeleteAsync (role);
        StatusMessage = "ĐãDeleted " + role.Name;

        return RedirectToPage ("Index");
      } else {
        Input.Name = role.Name;
        isConfirmed = true;

      }

      return Page ();
    }
  }
}