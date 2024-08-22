using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProgrammingTest.Models;

namespace ProgrammingTest.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: /Supplier/
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();
            return View(suppliers);
        }

        // GET: /Supplier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: /Supplier/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: /Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: /Supplier/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: /Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Supplier/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /Supplier/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var supplier = new Supplier
                        {
                            SupplierCode = worksheet.Cells[row, 1].Text,
                            SupplierName = worksheet.Cells[row, 2].Text,
                            Address = worksheet.Cells[row, 3].Text,
                            Province = worksheet.Cells[row, 4].Text,
                            City = worksheet.Cells[row, 5].Text,
                            PIC = worksheet.Cells[row, 6].Text

                        };

                        _context.Add(supplier);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Supplier/Download
        public IActionResult Download()
        {
            var suppliers = _context.Suppliers.ToList();
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Suppliers");

            worksheet.Cells[1, 1].Value = "Supplier Code";
            worksheet.Cells[1, 2].Value = "Supplier Name";
            worksheet.Cells[1, 3].Value = "Address";
            worksheet.Cells[1, 4].Value = "Province";
            worksheet.Cells[1, 5].Value = "City";
            worksheet.Cells[1, 6].Value = "PIC";

            for (int i = 0; i < suppliers.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = suppliers[i].SupplierCode;
                worksheet.Cells[i + 2, 2].Value = suppliers[i].SupplierName;
                worksheet.Cells[i + 2, 3].Value = suppliers[i].Address;
                worksheet.Cells[i + 2, 4].Value = suppliers[i].Province;
                worksheet.Cells[i + 2, 5].Value = suppliers[i].City;
                worksheet.Cells[i + 2, 6].Value = suppliers[i].PIC;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            var fileName = "Suppliers.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
