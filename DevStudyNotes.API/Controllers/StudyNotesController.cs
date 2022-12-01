using DevStudyNotes.API.Entities;
using DevStudyNotes.API.Models;
using DevStudyNotes.API.Persistnece;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevStudyNotes.API.Controllers
{
    [ApiController]
    [Route("api/study-notes")]
    public class StudyNotesController : ControllerBase
    {
        private readonly StudyNoteDbContext _context;

        public StudyNotesController(StudyNoteDbContext context)
        {
            this._context = context;
        }
        // api/study-notes
        [HttpGet]
        public IActionResult GetAll()
        {
            var studyNotes = _context.StudyNotes.ToList();

            return Ok(studyNotes);
        }

        // api/study-notes/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            // talvez retorna NotFound
            var studyNote = _context.StudyNotes
                .Include(s => s.Reactions)
                .SingleOrDefault(s => s.Id == id);

            if (studyNote == null)
            {
                return NotFound();
            }
            return Ok(studyNote);
        }

        // api/study-notes
        [HttpPost]
        public IActionResult Post(AddStudyNoteInputModel model)
        {
            // talvez retorne bad request
            var studyNote = new StudyNote(model.Title, model.Description, model.isPublic);

            _context.StudyNotes.Add(studyNote);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = studyNote.Id }, model);
        }

        // api/study-notes/1/reactions
        [HttpPost("{id}/reactions")]
        public IActionResult PostReaction(int id, AddReactionStudyNoteInputModel model) 
        {
            // Talvez not found / bad request
            var studyNotes = _context.StudyNotes.SingleOrDefault(s => s.Id == id);

            if(studyNotes == null)
            {
                return BadRequest();
            }

            studyNotes.AddReaction(model.IsPositive);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
