using System.ComponentModel.DataAnnotations;
using TestProjectLatvia.Domains;

namespace TestProjectLatvia.ViewModels;

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public EStatus Status { get; set; }
}