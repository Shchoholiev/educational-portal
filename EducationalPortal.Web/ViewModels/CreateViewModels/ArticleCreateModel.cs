﻿using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Web.ViewModels.CreateViewModels
{
    public class ArticleCreateModel : MaterialsBaseCreateModel
    {
        public Resource Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
