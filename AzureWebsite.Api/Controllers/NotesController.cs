using AzureWebsite.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzureWebsite.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly ILogger<NotesController> logger;
    private readonly NotesDb db;

    public NotesController(ILogger<NotesController> logger, NotesDb db)
    {
        this.logger = logger;
        this.db = db;
    }



    // GET /api/notes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetAllNotes()
    {
        return await db.Notes.ToListAsync();
    }

    // GET /api/notes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNoteById(int id)
    {
        var note = await db.Notes.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return note;
    }

    // POST /api/notes
    [HttpPost]
    public async Task<ActionResult<Note>> CreateNote(Note note)
    {
        db.Notes.Add(note);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
    }

    // PUT /api/notes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, Note note)
    {
        if (id != note.Id)
        {
            return BadRequest();
        }

        db.Entry(note).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NoteExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE /api/notes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var note = await db.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        db.Notes.Remove(note);
        await db.SaveChangesAsync();

        return NoContent();
    }

    private bool NoteExists(int id)
    {
        return db.Notes.Any(e => e.Id == id);
    }


}
