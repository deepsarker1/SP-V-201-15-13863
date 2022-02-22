#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeepWebApp.DBContexts;
using DeepWebApp.Models;

namespace DeepWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDBContext _context;

        public StudentsController(StudentDBContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Student.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,Name,BirthDate,Age,Gender,PhoneNo,Email,University")] Student student)
        {
            if (ModelState.IsValid)
            {
                //Custom Code Added By DS
                foreach (Student data in _context.Student.ToList())
                {
                    if (data.StudentId == student.StudentId)
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }
                //Custom Code Added By DS
                student.Id = ""+ObjIdTracker.CurrentId;

                student.Age = ageCalculator(student.BirthDate);

                _context.Add(student);
                await _context.SaveChangesAsync();
                //Custom Code Added By DS
                ObjIdTracker.CurrentId += 1;
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,StudentId,Name,BirthDate,Age,Gender,PhoneNo,Email,University")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string editedId = student.Id;
                string editedStudentId = student.StudentId;
                //Custom Code Added By DS
                foreach (Student data in _context.Student.AsNoTracking().ToList())
                //AsNoTracking Let The Data Not To Create Instance. Otherwiswe In If Condition ID CanNot Be Checked.
           
                { //Here I Checked If Both The Student Id Is Same Or Not. If Same Then Their Obj Id Is Same Or Not.
                    // If Both Returns True That Means No New Student Id Is Edited And So Rest Of Any Data Can be Edited.
                    //If False Means The User Tried To Change StudentId But That StudentID Already Exist In Database. 
                    if (editedStudentId == data.StudentId && editedId != data.Id) { return RedirectToAction(nameof(Index));}
                }

                try
                {
                    student.Age = ageCalculator(student.BirthDate); 
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.Id == id);
        }

        private int ageCalculator(string data)
        {
            string[] BirthDateSplitted = data.Split("-");
            string[] CurrentDateSplitted = DateTime.Now.ToString("d-M-yyyy").Split("-");
            int age = Int16.Parse(CurrentDateSplitted[2]) - Int16.Parse(BirthDateSplitted[2]);
            bool decrease = false;
            if (Int16.Parse(CurrentDateSplitted[1]) < Int16.Parse(BirthDateSplitted[1])) { decrease = true; }
            if (Int16.Parse(CurrentDateSplitted[1]) == Int16.Parse(BirthDateSplitted[1]))
            {
                if (Int16.Parse(CurrentDateSplitted[0]) < Int16.Parse(BirthDateSplitted[0])) { decrease = true; }
            }
            if (decrease){ age -= 1;}
            return age;
        }
    }
}
