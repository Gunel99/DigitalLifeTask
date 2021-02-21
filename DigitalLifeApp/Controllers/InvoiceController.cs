using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalLifeApp.DAL;
using DigitalLifeApp.Models;
using DigitalLifeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DigitalLifeApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly AppDbContext context;
        public InvoiceController(AppDbContext _context)
        {
            context = _context;
        }

        //public IEnumerable<Invoice> results { get; set; }

        public IActionResult List()
        {
            return View(context.Invoices.Include(i => i.Client).Include(i => i.Project).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> List(string ProjectSearch)
        {
            ViewData["GetInvoiceDetails"] = ProjectSearch;

            var projectQuery = from x in context.Invoices.Include(i => i.Client).Include(i => i.Project) select x;

            if (!String.IsNullOrEmpty(ProjectSearch))
            {
                projectQuery = projectQuery.Where(x => x.Project.Name.StartsWith(ProjectSearch) ||
                x.Client.Name.StartsWith(ProjectSearch));
            }
            //results = context.Invoices.Include(i => i.Client).Include(i => i.Project).ToList();
            //ViewBag.Results = results;
            return View(await projectQuery.AsNoTracking().ToListAsync());
        }

        //[HttpPost]
        //public IActionResult List(DateTime startTime, DateTime endTime)
        //{            
        //    results = (from x in context.Invoices.Include(i => i.Client).Include(i => i.Project)
        //               where (x.InvoiceDate >= startTime) &&
        //                     (x.InvoiceDate <= endTime)
        //               select x).ToList();

        //    ViewBag.Results = results;

        //    return View();
        //}

        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return NotFound();

            Invoice invoice = await context.Invoices.Include(i => i.Client).Include(i => i.Project).Where(i => i.Id == id).SingleOrDefaultAsync();
            return View(invoice);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(context.Clients.ToList(), "Id", "Name");
            ViewBag.Projects = new SelectList(context.Projects.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                context.Invoices.Add(invoice);
                await context.SaveChangesAsync();
                return RedirectToAction("List");
            }

            ViewBag.Clients = new SelectList(context.Clients.ToList(), "Id", "Name");
            ViewBag.Projects = new SelectList(context.Projects.ToList(), "Id", "Name");

            return View(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Clients = new SelectList(context.Clients.ToList(), "Id", "Name");
            ViewBag.Projects = new SelectList(context.Projects.ToList(), "Id", "Name");

            Invoice invoice = await context.Invoices.Include(i => i.Client).Include(i => i.Project).FirstOrDefaultAsync(i => i.Id == id);

            return View(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                context.Invoices.Update(invoice);
                await context.SaveChangesAsync();
                return RedirectToAction("List");
            }

            ViewBag.Clients = new SelectList(context.Clients.ToList(), "Id", "Name");
            ViewBag.Projects = new SelectList(context.Projects.ToList(), "Id", "Name");

            return View(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Invoice invoice = context.Invoices.Find(id);
            context.Invoices.Remove(invoice);
            await context.SaveChangesAsync();
            return RedirectToAction("List");
        }

    }
}
