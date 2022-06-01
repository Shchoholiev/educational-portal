using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.API.ViewModels
{
    public class VideoViewModel : MaterialsBaseViewModel
    {
        public DateTime Duration { get; set; }

        public Quality Quality { get; set; }
    }
}
