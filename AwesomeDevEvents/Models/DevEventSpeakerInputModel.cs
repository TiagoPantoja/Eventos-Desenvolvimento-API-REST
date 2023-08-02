namespace AwesomeDevEvents.Models;

public class DevEventSpeakerInputModel
{
    public Guid Id { get; set; }
    public Guid DevEventId { get; set; }
    public string Name { get; set; }
    public string TalkTitle { get; set; }
    public string TalkDescription { get; set; }
    public string LinkedInProfile { get; set; }
}