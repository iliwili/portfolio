using System.ComponentModel.DataAnnotations;

namespace Portfolio.Business.Emails.Models;

public class MailRequest
{
    [Required]
    public ICollection<Recipient> To { get; set; } = [];
    public ICollection<Recipient>? Cc { get; set; }
    public ICollection<Recipient>? Bcc { get; set; }
    public string? Subject { get; set; }
    public string? HtmlContent { get; set; }
    public long? TemplateId { get; set; }
    public Dictionary<string, object>? Params { get; set; }
    public List<Attachment>? Attachments { get; set; }
}

public class Recipient
{
    public required string Email { get; set; }
    public required string Name { get; set; }
}

public class Attachment
{
    public string? Url { get; set; } // Option 1: let Brevo fetch from URL
    public byte[]? Content { get; set; } // Option 2: base64 encoded file
    public string Name { get; set; } = string.Empty;
}