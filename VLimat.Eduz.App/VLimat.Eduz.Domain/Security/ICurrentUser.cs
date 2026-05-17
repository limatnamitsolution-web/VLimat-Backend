//csharp VLimat.Eduz.Domain\Security\ICurrentUser.cs
namespace VLimat.Eduz.Domain.Security
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        int? UserId { get; }
        string UserName { get; }

        int? AcademicId { get; }
        string AcademicName { get; }

        int? FyId { get; }
        string Fy { get; }

        /// <summary>
        /// Allows manual population for non-HTTP scenarios (background jobs, tests).
        /// Keep this optional — prefer claims-based population.
        /// </summary>
        void Set(
            int? userId = null,
            string? userName = null,
            int? academicId = null,
            string? academicName = null,
            int? fyId = null,
            string? fy = null);
    }
}