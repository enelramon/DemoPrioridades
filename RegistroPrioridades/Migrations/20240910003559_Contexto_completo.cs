using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroPrioridades.Migrations
{
    /// <inheritdoc />
    public partial class Contexto_completo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    AvailabilityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvailabilityType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Availabi__DA3979B1DFA9D056", x => x.AvailabilityId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Language__B93855ABFC102DD2", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "PreferredWorkType",
                columns: table => new
                {
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkTypeName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Preferre__CCC06D2098F48FFF", x => x.WorkTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SkillCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SkillCat__19093A0BAAEC6961", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    UserType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "(getdate())"),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4CE9E3C269", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SkillName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Skills__DFA09187BF73FFB2", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK__Skills__Category__36B12243",
                        column: x => x.CategoryId,
                        principalTable: "SkillCategory",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Industry = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CompanyP__2D971CAC66B3E5FD", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK__CompanyPr__UserI__4D94879B",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Biography = table.Column<string>(type: "TEXT", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    DesiredSalary = table.Column<decimal>(type: "decimal(10, 2)", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    AvailabilityId = table.Column<int>(type: "INTEGER", nullable: true),
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    GitHubProfile = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    LinkedInProfile = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PortfolioUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserProf__290C88E412B6A8C3", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK__UserProfi__Avail__2F10007B",
                        column: x => x.AvailabilityId,
                        principalTable: "Availability",
                        principalColumn: "AvailabilityId");
                    table.ForeignKey(
                        name: "FK__UserProfi__UserI__2E1BDC42",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK__UserProfi__WorkT__300424B4",
                        column: x => x.WorkTypeId,
                        principalTable: "PreferredWorkType",
                        principalColumn: "WorkTypeId");
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    JobOfferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    SalaryRangeMin = table.Column<decimal>(type: "decimal(10, 2)", nullable: true),
                    SalaryRangeMax = table.Column<decimal>(type: "decimal(10, 2)", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    JobType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "(getdate())"),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__JobOffer__5B32794DD5D83230", x => x.JobOfferId);
                    table.ForeignKey(
                        name: "FK__JobOffers__Compa__52593CB8",
                        column: x => x.CompanyId,
                        principalTable: "CompanyProfiles",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK__JobOffers__WorkT__534D60F1",
                        column: x => x.WorkTypeId,
                        principalTable: "PreferredWorkType",
                        principalColumn: "WorkTypeId");
                });

            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    CertificationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    CertificationName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    IssuingOrganization = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    IssueDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CredentialId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CredentialUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Certific__1237E58AE2306722", x => x.CertificationId);
                    table.ForeignKey(
                        name: "FK__Certifica__Profi__44FF419A",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    InstitutionName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Degree = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    FieldOfStudy = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Educatio__4BBE38050FD5C6ED", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK__Education__Profi__4AB81AF0",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "UserLanguages",
                columns: table => new
                {
                    UserLanguageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProficiencyLevel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserLang__8086CE39FFCBE055", x => x.UserLanguageId);
                    table.ForeignKey(
                        name: "FK__UserLangu__Langu__4222D4EF",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId");
                    table.ForeignKey(
                        name: "FK__UserLangu__Profi__412EB0B6",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    UserSkillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: true),
                    SkillLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSkil__2F28BE56474887EF", x => x.UserSkillId);
                    table.ForeignKey(
                        name: "FK__UserSkill__Profi__3A81B327",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId");
                    table.ForeignKey(
                        name: "FK__UserSkill__Skill__3B75D760",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    ExperienceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkExpe__2F4E3449351807F6", x => x.ExperienceId);
                    table.ForeignKey(
                        name: "FK__WorkExper__Profi__47DBAE45",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobOfferId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CoverLetter = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__JobAppli__C93A4C99F531AC3F", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK__JobApplic__JobOf__5BE2A6F2",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "JobOfferId");
                    table.ForeignKey(
                        name: "FK__JobApplic__Profi__5CD6CB2B",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "JobSkillRequirements",
                columns: table => new
                {
                    JobSkillReqId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobOfferId = table.Column<int>(type: "INTEGER", nullable: true),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: true),
                    RequiredLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__JobSkill__9685A56A44B15020", x => x.JobSkillReqId);
                    table.ForeignKey(
                        name: "FK__JobSkillR__JobOf__571DF1D5",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "JobOfferId");
                    table.ForeignKey(
                        name: "FK__JobSkillR__Skill__5812160E",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Availabi__AF78582AF5049A9C",
                table: "Availability",
                column: "AvailabilityType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_ProfileId",
                table: "Certifications",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_UserId",
                table: "CompanyProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_ProfileId",
                table: "Education",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobOfferId",
                table: "JobApplications",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ProfileId",
                table: "JobApplications",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CompanyId",
                table: "JobOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_WorkTypeId",
                table: "JobOffers",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkillRequirements_JobOfferId",
                table: "JobSkillRequirements",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkillRequirements_SkillId",
                table: "JobSkillRequirements",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "UQ__Language__E89C4A6AF31DE079",
                table: "Languages",
                column: "LanguageName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Preferre__4C45DD915E616D8D",
                table: "PreferredWorkType",
                column: "WorkTypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SkillCat__8517B2E0AD612525",
                table: "SkillCategory",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CategoryId",
                table: "Skills",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ__Skills__B63C657179F210A0",
                table: "Skills",
                column: "SkillName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_LanguageId",
                table: "UserLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_ProfileId",
                table: "UserLanguages",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AvailabilityId",
                table: "UserProfiles",
                column: "AvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_WorkTypeId",
                table: "UserProfiles",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D1053416007C50",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_ProfileId",
                table: "UserSkills",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_ProfileId",
                table: "WorkExperience",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "JobSkillRequirements");

            migrationBuilder.DropTable(
                name: "UserLanguages");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "CompanyProfiles");

            migrationBuilder.DropTable(
                name: "SkillCategory");

            migrationBuilder.DropTable(
                name: "Availability");

            migrationBuilder.DropTable(
                name: "PreferredWorkType");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
