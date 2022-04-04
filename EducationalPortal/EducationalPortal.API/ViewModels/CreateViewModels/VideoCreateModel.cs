using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.API.ViewModels.CreateViewModels
{
    public class VideoCreateModel : MaterialsBaseCreateModel
    {
        public DateTime Duration { get; set; }

        public Quality Quality { get; set; }
    }
}
