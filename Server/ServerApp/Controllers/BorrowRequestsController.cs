using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    public class BorrowRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public BorrowRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BorrowRequests
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BorrowRequests.Include(b => b.Book).Include(b => b.Librarian);
            return View(await appDbContext.ToListAsync());
        }

        // GET: BorrowRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowRequest = await _context.BorrowRequests
                .Include(b => b.Book)
                .Include(b => b.Librarian)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (borrowRequest == null)
            {
                return NotFound();
            }

            return View(borrowRequest);
        }

        // GET: BorrowRequests/Create
        public IActionResult Create()
        {
            ViewBag.Librarians = new SelectList(_context.Librarians, "ID", "Name");
            ViewBag.Books = new SelectList(_context.Books, "ID", "Title");
            return View();
        }

        // POST: BorrowRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LibrarianID,BookID,RequestDate,Status")] BorrowRequest borrowRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Librarians = new SelectList(_context.Librarians, "ID", "Name", borrowRequest.LibrarianID);
            ViewBag.Books = new SelectList(_context.Books, "ID", "Title", borrowRequest.BookID);
            return View(borrowRequest);
        }


        // GET: BorrowRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowRequest = await _context.BorrowRequests.FindAsync(id);
            if (borrowRequest == null)
            {
                return NotFound();
            }

            ViewBag.Librarians = new SelectList(_context.Librarians, "ID", "Name", borrowRequest.LibrarianID);
            ViewBag.Books = new SelectList(_context.Books, "ID", "Title", borrowRequest.BookID);
            return View(borrowRequest);
        }


        // POST: BorrowRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LibrarianID,BookID,RequestDate,Status")] BorrowRequest borrowRequest)
        {
            if (id != borrowRequest.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowRequestExists(borrowRequest.ID))
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

            ViewBag.Librarians = new SelectList(_context.Librarians, "ID", "Name", borrowRequest.LibrarianID);
            ViewBag.Books = new SelectList(_context.Books, "ID", "Title", borrowRequest.BookID);
            return View(borrowRequest);
        }


        // GET: BorrowRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowRequest = await _context.BorrowRequests
                .Include(b => b.Book)
                .Include(b => b.Librarian)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (borrowRequest == null)
            {
                return NotFound();
            }

            return View(borrowRequest);
        }

        // POST: BorrowRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowRequest = await _context.BorrowRequests.FindAsync(id);
            if (borrowRequest != null)
            {
                _context.BorrowRequests.Remove(borrowRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowRequestExists(int id)
        {
            return _context.BorrowRequests.Any(e => e.ID == id);
        }
    }
}
