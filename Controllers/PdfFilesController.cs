using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace PMApp.Controllers
{
    public class PdfFilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _env;

        public PdfFilesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: PdfFiles
        public async Task<IActionResult> Index(int id)
        {
            var pdfs = from p in _context.PdfFile where p.TID == id select p;

            ViewBag.Tenant = id;
            return View(await pdfs.ToListAsync());
        }

        public IActionResult Create(int TID)
        {
            var tenants = from t in _context.Tenant where t.TID == TID select t;
            ViewData["TID"] = new SelectList(tenants, "TID", "TID");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, PdfFile pdf)
        {
            if (file == null)
            {
                return View();
            }

            if (file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "file", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                pdf.FileName = fileName;
            }
            _context.PdfFile.Add(pdf);
            ViewBag.TID = pdf.TID;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "PdfFiles", new { id = pdf.TID});
        }

        [HttpGet]
        public ActionResult GetPdf(string fileName)
        {
            string filePath = "~/file/" + fileName;
            Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);
            return File(filePath, "application/pdf");
        }

        // GET: PdfFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pdfFile = await _context.PdfFile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pdfFile == null)
            {
                return NotFound();
            }

            return View(pdfFile);
        }

        // POST: PdfFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var pdfFile = await _context.PdfFile.FindAsync(id);
            string filePath = "~/file/" + pdfFile.FileName;

            string webRootPath = _env.WebRootPath;
            var fileName = pdfFile.FileName;
            var fullPath = webRootPath + "/file/" + fileName;

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            System.IO.File.Delete(fullPath); 

            _context.PdfFile.Remove(pdfFile);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "PdfFiles", new { id = pdfFile.TID });
        }

        private bool PdfFileExists(int id)
        {
            return _context.PdfFile.Any(e => e.Id == id);
        }
    }
}
