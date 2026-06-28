using System.ComponentModel.DataAnnotations;
namespace Cityinfo.API.Models
{
    //Fluent validation
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "name ra vared konid")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
