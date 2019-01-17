using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class SavedGameChoiceModel : PageModel
    {
        public AppDbContext context;
        
        [BindProperty] public int Index { get; set; }
        
        public void OnGet(int index)
        {
            
            Index = index;
        }
        
        
        public IActionResult OnPost(string submit)
        {
            if (submit.Equals("Load Game"))
            {
                return RedirectToPage("PlayingGame", new {index = Index, message = "", move = true});
            }
            return Page();
        }

    }
}