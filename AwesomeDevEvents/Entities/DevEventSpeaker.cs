namespace AwesomeDevEvents.Entities;

public class DevEventSpeaker
{
    public Guid Id { get; set; }
    public Guid DevEventId { get; set; }
    public String Name { get; set; }
    public String TalkTitle { get; set; }
    public String TalkDescription { get; set; }
    public String LinkedInProfile { get; set; }
}