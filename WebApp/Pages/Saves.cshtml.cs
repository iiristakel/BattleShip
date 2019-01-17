using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages
{
    public class SavesModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public SavesModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; }
        

        public async Task OnGetAsync()
        {
            Game = await _context.Games
                .Include(g => g.PlayerOne)
                .Include(g => g.PlayerTwo).ToListAsync();
        }
        
    }
}
