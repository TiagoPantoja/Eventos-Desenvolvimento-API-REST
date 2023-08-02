using AutoMapper;
using AwesomeDevEvents.Entities;
using AwesomeDevEvents.Models;
using AwesomeDevEvents.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.Controllers;

[Route("api/dev-events")]
[ApiController]
public class DevEventsController : ControllerBase
{
    private readonly DevEventsDbContext _context;
    private readonly IMapper _mapper;
    
    public DevEventsController(DevEventsDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // api/dev-events (GET)
    [HttpGet]
    public IActionResult GetAll()
    {
        var devEvents = _context.DevEvents.Where(d => d.IsDeleted).ToList();
        
        var viewModel = _mapper.Map<List<DevEventViewModel>>(devEvents);
        return Ok(viewModel);
    }
    
    // api/dev-events/1 (GET ID)
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var devEvent = _context.DevEvents
            .Include(de => de.Speakers)
            .SingleOrDefault(d => d.Id == id);

        if (devEvent == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<DevEventViewModel>(devEvent);
        
        return Ok(viewModel);
    }
    
    // api/dev-events (POST)
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post(DevEventInputModel input)
    {
        var devEvent = _mapper.Map<DevEvent>(input);
        
        _context.DevEvents.Add(devEvent);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
    }
    
    // api/dev-events/1 (PUT)
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Update(Guid id, DevEventInputModel input)
    {
        var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
        
        if (devEvent == null)
        {
            return NotFound();
        }
        
        devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);
        
        _context.DevEvents.Update(devEvent);
        _context.SaveChanges();

        return NoContent();
    }
    
    // api/dev-events/1 (DELETE)
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(Guid id)
    {
        var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);

        if (devEvent == null)
        {
            return NotFound();
        }
        
        devEvent.Delete();

        _context.SaveChanges();
        
        return NoContent();
    }
    
    // api/dev-events/1/speakers (POST)
    [HttpPost("{id}/speakers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult PostSpeaker(Guid id,DevEventSpeakerInputModel input)
    {
        var speaker = _mapper.Map<DevEventSpeaker>(input);
        speaker.DevEventId = id;
        
        var devEvent = _context.DevEvents.Any(d => d.Id == id);

        if (!devEvent)
        {
            return NotFound();
        }

        _context.DevEventSpeakers.Add(speaker);
        _context.SaveChanges();
        
        return NoContent();
    }
}