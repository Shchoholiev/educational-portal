using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.Courses;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class CertificatesRepository : ICertificatesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<Certificate> _table;

        public CertificatesRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = this._db.Certificates;
        }

        public async Task AddAsync(Certificate certificate, CancellationToken cancellationToken)
        {
            this._db.Attach(certificate);
            await this._table.AddAsync(certificate, cancellationToken);
            await this.SaveAsync(cancellationToken);
        }

        public Task<Certificate?> GetCertificateAsync(int courseId, string userId, CancellationToken cancellationToken)
        {
            return this._table
                .Include(c => c.Course)
                    .ThenInclude(c => c.Author)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CourseId == courseId && c.UserId == userId, cancellationToken);
        }

        public Task<Certificate?> GetCertificateAsync(Guid verificationCode, CancellationToken cancellationToken)
        {
            return this._table
                .Include(c => c.Course)
                    .ThenInclude(c => c.Author)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.VerificationCode == verificationCode, cancellationToken);
        }

        public Task<bool> ExistsAsync(int courseId, string userId, CancellationToken cancellationToken)
        {
            return this._table.AnyAsync(c => c.CourseId == courseId && c.UserId == userId, cancellationToken);
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
