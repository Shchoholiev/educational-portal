using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Core.Entities.Courses;
using EducationalPortal.Infrastructure.CustomMiddlewares;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services
{
    public class CertificatesService : ICertificatesService
    {
        private readonly ICertificatesRepository _cetificatesRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly ILogger _logger;

        private const string _certificateUrl = "https://educationalportal.blob.core.windows.net/essentials/Certificate%20of%20Completion.pdf";
        
        private const string _robotoSlabBold = "https://educationalportal.blob.core.windows.net/essentials/RobotoSlab-Bold.ttf";

        public CertificatesService(ICertificatesRepository cetificatesRepository,
                                   IUsersCoursesRepository usersCoursesRepository, 
                                   ILogger<CertificatesService> logger)
        {
            _cetificatesRepository = cetificatesRepository;
            _usersCoursesRepository = usersCoursesRepository;
            _logger = logger;
        }

        public async Task CreateAsync(int courseId, string userId, CancellationToken cancellationToken)
        {
            if (!await _usersCoursesRepository.ExistsByIdsAsync(courseId, userId, cancellationToken))
            {
                throw new NotFoundException("UserCourse");
            }

            var certificate = new Certificate
            {
                CourseId = courseId,
                UserId = userId,
                VerificationCode = Guid.NewGuid(),
                DateOfCompletionUTC = DateTime.UtcNow,
            };

            await _cetificatesRepository.AddAsync(certificate, cancellationToken);
        }

        public async Task<byte[]> GetPdfAsync(int courseId, string userId, CancellationToken cancellationToken)
        {
            var certificate = await _cetificatesRepository.GetCertificateAsync(courseId, userId, cancellationToken);
            if (certificate == null)
            {
                throw new NotFoundException("Certificate");
            }

            return this.GeneratePdf(certificate);
        }

        public async Task<byte[]> VerifyAsync(Guid verificationCode, CancellationToken cancellationToken)
        {
            var certificate = await _cetificatesRepository.GetCertificateAsync(verificationCode, cancellationToken);
            if (certificate == null)
            {
                throw new NotFoundException("Certificate");
            }

            return this.GeneratePdf(certificate);
        }

        public Task<bool> ExistsAsync(int courseId, string userId, CancellationToken cancellationToken)
        {
            return _cetificatesRepository.ExistsAsync(courseId, userId, cancellationToken);
        }

        private byte[] GeneratePdf(Certificate certificate)
        {
            using var reader = new PdfReader(_certificateUrl);
            using var newFile = new MemoryStream();
            using var writer = new PdfWriter(newFile);
            using var pdf = new PdfDocument(reader, writer);

            var page = pdf.GetPage(1);
            var canvas = new PdfCanvas(page);

            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
                .SetFillColor(new DeviceRgb(42, 44, 62))
                .MoveText(90, 380)
                .ShowText(certificate.DateOfCompletionUTC.ToString("MMM dd, yyyy"))
                .EndText();

            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(_robotoSlabBold), 24)
                .SetFillColor(new DeviceRgb(42, 44, 62))
                .MoveText(90, 355)
                .ShowText(certificate.User.Name.ToUpper())
                .EndText();

            var rows = 0;
            var lastWhiteSpace = 0;
            for (int i = 0; i < certificate.Course.Name.Length; i += 38)
            {
                var index = 0;
                if (certificate.Course.Name.Length - lastWhiteSpace < i + 38)
                {
                    index = certificate.Course.Name.Length;
                }
                else
                {
                    index = certificate.Course.Name[lastWhiteSpace..(lastWhiteSpace + 39)].LastIndexOf(" ");
                }
                rows++;

                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(_robotoSlabBold), 18)
                    .SetFillColor(new DeviceRgb(42, 44, 62))
                    .MoveText(90, 300 - i / 2)
                    .ShowText(certificate.Course.Name[lastWhiteSpace..index])
                    .EndText();

                lastWhiteSpace = index + 1;
            }

            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
                .SetFillColor(new DeviceRgb(42, 44, 62))
                .MoveText(90, 300 - rows * 20)
                .ShowText($"by {certificate.Course.Author.Name} - {certificate.Course.Author.Position}")
                .EndText();

            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 10)
                .SetFillColor(new DeviceRgb(226, 120, 247))
                .MoveText(90, 38)
                .ShowText($"{AppHttpContext.BaseUrl}/api/certificates/verify/{certificate.VerificationCode}")
                .EndText();

            pdf.Close();

            return newFile.ToArray();
        }
    }
}
