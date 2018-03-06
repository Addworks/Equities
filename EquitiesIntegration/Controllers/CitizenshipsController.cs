using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EquitiesIntegration.Models;

namespace EquitiesIntegration.Controllers
{
    public class CitizenshipsController : Controller
    {
        private EquitiesIntegrationContext db = new EquitiesIntegrationContext();

        // GET: Citizenships
        public async Task<ActionResult> Index()
        {
            return View(await db.Citizenships.ToListAsync());
        }

        // GET: Citizenships/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citizenship citizenship = await db.Citizenships.FindAsync(id);
            if (citizenship == null)
            {
                return HttpNotFound();
            }
            return View(citizenship);
        }

        // GET: Citizenships/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Citizenships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CitizenshipId,Name")] Citizenship citizenship)
        {
            if (ModelState.IsValid)
            {
                db.Citizenships.Add(citizenship);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(citizenship);
        }

        // GET: Citizenships/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citizenship citizenship = await db.Citizenships.FindAsync(id);
            if (citizenship == null)
            {
                return HttpNotFound();
            }
            return View(citizenship);
        }

        // POST: Citizenships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CitizenshipId,Name")] Citizenship citizenship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citizenship).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(citizenship);
        }

        // GET: Citizenships/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citizenship citizenship = await db.Citizenships.FindAsync(id);
            if (citizenship == null)
            {
                return HttpNotFound();
            }
            return View(citizenship);
        }

        // POST: Citizenships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Citizenship citizenship = await db.Citizenships.FindAsync(id);
            db.Citizenships.Remove(citizenship);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
