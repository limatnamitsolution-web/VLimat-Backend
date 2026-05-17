
namespace VLimat.Eduz.Application.Features.MasterConfig.DTOs
{
    public class MasterConfigRequest
    {
        public int Id { get; set; }
        public int AcademicId { get; set; }
        public string Configuration { get; set; } = string.Empty;
        public string ConfigKey { get; set; } = string.Empty;
        public string ConfigValue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int AC_Yr { get; set; }
        public int CreatedBy { get; set; }
    }
}
