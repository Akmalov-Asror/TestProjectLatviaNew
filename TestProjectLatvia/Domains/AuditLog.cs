namespace TestProjectLatvia.Domains;

public class AuditLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the action performed, describing the event or operation.
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// Gets or sets the username associated with the action.
    /// <para>
    /// here the user name should be taken from the claim or saved by other methods
    /// </para>
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the name of the controller related to the action.
    /// </summary>
    public string? ControllerName { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the action occurred.
    /// </summary>
    public DateTime? DateTime { get; set; }

    /// <summary>
    /// Gets or sets the old value or state before the action.
    /// </summary>
    public string? OldValue { get; set; }

    /// <summary>
    /// Gets or sets the new value or state after the action.
    /// </summary>
    public string? NewValue { get; set; }
}