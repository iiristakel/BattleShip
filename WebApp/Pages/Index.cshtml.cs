using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet(int index)
        {
        }
        
        
        public IActionResult OnPost(string submit)
        {
            if (submit.Equals("New Game"))
                return RedirectToPage("NewGameSetup");
            if (submit.Equals("Load Game")) 
                return RedirectToPage("SavedGameChoice");
            if (submit.Equals("Saves"))
                return RedirectToPage("Saves");
            if (submit.Equals("Exit"))
                Environment.Exit(0);
            return Page();
        }

    }
}