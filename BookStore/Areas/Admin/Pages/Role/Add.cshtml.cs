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
    public class AddModel : PageModel {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddModel (RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
        }

        [TempData] 
        public string StatusMessage { get; set; }

        public class InputModel {
            public string ID { set; get; }

            [Required (ErrorMessage = "Must enter role name")]
            [Display (Name = "Name of Role")]
            [StringLength (100, ErrorMessage = "{0} long {2} to {1} characters.", MinimumLength = 3)]
            public string Name { set; get; }

        }
        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool IsUpdate { set; get; }

        public IActionResult OnGet () => NotFound ("Not found");
        public IActionResult OnPost () => NotFound ("Not found");
        public IActionResult OnPostStartNewRole () {
            StatusMessage = "Please enter information to create a new role";
            IsUpdate = false;
            ModelState.Clear ();
            return Page ();
        }
        public async Task<IActionResult> OnPostStartUpdate () {
            StatusMessage = null;
            IsUpdate = true;
            if (Input.ID == null) {
                StatusMessage = "Error: No information about Role";
                return Page ();
            }
            var result = await _roleManager.FindByIdAsync (Input.ID);
            if (result != null) {
                Input.Name = result.Name;
                ViewData["Title"] = "Update roles : " + Input.Name;
                ModelState.Clear ();
            } else {
                StatusMessage = "Error: No information about Role ID= " + Input.ID;
            }

            return Page ();
        }

        public async Task<IActionResult> OnPostAddOrUpdate () {

            if (!ModelState.IsValid) {
                StatusMessage = null;
                return Page ();
            }

            if (IsUpdate) {
                
                if (Input.ID == null) {
                    ModelState.Clear ();
                    StatusMessage = "Error: No information about role";
                    return Page ();
                }
                var result = await _roleManager.FindByIdAsync (Input.ID);
                if (result != null) {
                    result.Name = Input.Name;
                    var roleUpdateRs = await _roleManager.UpdateAsync (result);
                    if (roleUpdateRs.Succeeded) {
                        StatusMessage = "The role has been updated successfully";
                    } else {
                        StatusMessage = "Error: ";
                        foreach (var er in roleUpdateRs.Errors) {
                            StatusMessage += er.Description;
                        }
                    }
                } else {
                    StatusMessage = "Error: Updated Role not found";
                }

            } else {
              
                var newRole = new IdentityRole (Input.Name);
                var rsNewRole = await _roleManager.CreateAsync (newRole);
                if (rsNewRole.Succeeded) {
                    StatusMessage = $"New role created successfully: {newRole.Name}";
                    return RedirectToPage("./Index");
                } else {
                    StatusMessage = "Error: ";
                    foreach (var er in rsNewRole.Errors) {
                        StatusMessage += er.Description;
                    }
                }
            }

            return Page ();

        }
    }
}