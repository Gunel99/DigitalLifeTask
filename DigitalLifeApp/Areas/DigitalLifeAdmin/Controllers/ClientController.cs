using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalLifeApp.DAL;
using DigitalLifeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalLifeApp.Areas.DigitalLifeAdmin.Controllers
{
    [Area("DigitalLifeAdmin")]
    [Authorize(Roles = "Admin")]
    public class ClientController : Controller
    {
        private readonly AppDbContext context;
        public ClientController(AppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            return View(context.Clients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                context.Clients.Add(client);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Client client = await context.Clients.FirstOrDefaultAsync(i => i.Id == id);

            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Client client)
        {
            if (ModelState.IsValid)
            {
                context.Clients.Update(client);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Client client = context.Clients.Find(id);
            context.Clients.Remove(client);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
