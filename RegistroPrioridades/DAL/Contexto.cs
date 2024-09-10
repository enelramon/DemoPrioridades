using Microsoft.EntityFrameworkCore;
using RegistroPrioridades.Models;

namespace RegistroPrioridades.DAL;
public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Prioridades> Prioridades { get; set; }


    public virtual DbSet<Availability> Availability { get; set; }

    public virtual DbSet<Certifications> Certifications { get; set; }

    public virtual DbSet<CompanyProfiles> CompanyProfiles { get; set; }

    public virtual DbSet<Education> Education { get; set; }

    public virtual DbSet<JobApplications> JobApplications { get; set; }

    public virtual DbSet<JobOffers> JobOffers { get; set; }

    public virtual DbSet<JobSkillRequirements> JobSkillRequirements { get; set; }

    public virtual DbSet<Languages> Languages { get; set; }

    public virtual DbSet<PreferredWorkType> PreferredWorkType { get; set; }

    public virtual DbSet<SkillCategory> SkillCategory { get; set; }

    public virtual DbSet<Skills> Skills { get; set; }

    public virtual DbSet<UserLanguages> UserLanguages { get; set; }

    public virtual DbSet<UserProfiles> UserProfiles { get; set; }

    public virtual DbSet<UserSkills> UserSkills { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<WorkExperience> WorkExperience { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PK__Availabi__DA3979B1DFA9D056");
        });

        modelBuilder.Entity<Certifications>(entity =>
        {
            entity.HasKey(e => e.CertificationId).HasName("PK__Certific__1237E58AE2306722");

            entity.HasOne(d => d.Profile).WithMany(p => p.Certifications).HasConstraintName("FK__Certifica__Profi__44FF419A");
        });

        modelBuilder.Entity<CompanyProfiles>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__CompanyP__2D971CAC66B3E5FD");

            entity.HasOne(d => d.User).WithMany(p => p.CompanyProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyPr__UserI__4D94879B");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.EducationId).HasName("PK__Educatio__4BBE38050FD5C6ED");

            entity.HasOne(d => d.Profile).WithMany(p => p.Education).HasConstraintName("FK__Education__Profi__4AB81AF0");
        });

        modelBuilder.Entity<JobApplications>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__JobAppli__C93A4C99F531AC3F");

            entity.Property(e => e.ApplicationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.JobOffer).WithMany(p => p.JobApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__JobOf__5BE2A6F2");

            entity.HasOne(d => d.Profile).WithMany(p => p.JobApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__Profi__5CD6CB2B");
        });

        modelBuilder.Entity<JobOffers>(entity =>
        {
            entity.HasKey(e => e.JobOfferId).HasName("PK__JobOffer__5B32794DD5D83230");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PostedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Company).WithMany(p => p.JobOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobOffers__Compa__52593CB8");

            entity.HasOne(d => d.WorkType).WithMany(p => p.JobOffers).HasConstraintName("FK__JobOffers__WorkT__534D60F1");
        });

        modelBuilder.Entity<JobSkillRequirements>(entity =>
        {
            entity.HasKey(e => e.JobSkillReqId).HasName("PK__JobSkill__9685A56A44B15020");

            entity.Property(e => e.IsRequired).HasDefaultValue(true);

            entity.HasOne(d => d.JobOffer).WithMany(p => p.JobSkillRequirements).HasConstraintName("FK__JobSkillR__JobOf__571DF1D5");

            entity.HasOne(d => d.Skill).WithMany(p => p.JobSkillRequirements).HasConstraintName("FK__JobSkillR__Skill__5812160E");
        });

        modelBuilder.Entity<Languages>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Language__B93855ABFC102DD2");
        });

        modelBuilder.Entity<PreferredWorkType>(entity =>
        {
            entity.HasKey(e => e.WorkTypeId).HasName("PK__Preferre__CCC06D2098F48FFF");
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__SkillCat__19093A0BAAEC6961");
        });

        modelBuilder.Entity<Skills>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__DFA09187BF73FFB2");

            entity.HasOne(d => d.Category).WithMany(p => p.Skills).HasConstraintName("FK__Skills__Category__36B12243");
        });

        modelBuilder.Entity<UserLanguages>(entity =>
        {
            entity.HasKey(e => e.UserLanguageId).HasName("PK__UserLang__8086CE39FFCBE055");

            entity.HasOne(d => d.Language).WithMany(p => p.UserLanguages).HasConstraintName("FK__UserLangu__Langu__4222D4EF");

            entity.HasOne(d => d.Profile).WithMany(p => p.UserLanguages).HasConstraintName("FK__UserLangu__Profi__412EB0B6");
        });

        modelBuilder.Entity<UserProfiles>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__UserProf__290C88E412B6A8C3");

            entity.HasOne(d => d.Availability).WithMany(p => p.UserProfiles).HasConstraintName("FK__UserProfi__Avail__2F10007B");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserProfi__UserI__2E1BDC42");

            entity.HasOne(d => d.WorkType).WithMany(p => p.UserProfiles).HasConstraintName("FK__UserProfi__WorkT__300424B4");
        });

        modelBuilder.Entity<UserSkills>(entity =>
        {
            entity.HasKey(e => e.UserSkillId).HasName("PK__UserSkil__2F28BE56474887EF");

            entity.HasOne(d => d.Profile).WithMany(p => p.UserSkills).HasConstraintName("FK__UserSkill__Profi__3A81B327");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills).HasConstraintName("FK__UserSkill__Skill__3B75D760");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE9E3C269");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<WorkExperience>(entity =>
        {
            entity.HasKey(e => e.ExperienceId).HasName("PK__WorkExpe__2F4E3449351807F6");

            entity.HasOne(d => d.Profile).WithMany(p => p.WorkExperience).HasConstraintName("FK__WorkExper__Profi__47DBAE45");
        });

    }

}

