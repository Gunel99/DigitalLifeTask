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
    public class ProjectController : Controller
    {
        private readonly AppDbContext context;
        public ProjectController(AppDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            return View(context.Projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                context.Projects.Add(project);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Project project = await context.Projects.FirstOrDefaultAsync(i => i.Id == id);

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Project project)
        {
            if (ModelState.IsValid)
            {
                context.Projects.Update(project);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Project project = context.Projects.Find(id);
            context.Projects.Remove(project);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
