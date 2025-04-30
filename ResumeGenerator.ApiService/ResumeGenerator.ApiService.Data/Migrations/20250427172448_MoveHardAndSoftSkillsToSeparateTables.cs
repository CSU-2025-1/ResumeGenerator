using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeGenerator.ApiService.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveHardAndSoftSkillsToSeparateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_user_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "hard_skills",
                table: "users");

            migrationBuilder.DropColumn(
                name: "soft_skills",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "resumes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_resumes",
                table: "resumes",
                column: "id");

            migrationBuilder.CreateTable(
                name: "hard_skills",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hard_skill_name = table.Column<string>(type: "text", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hard_skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_hard_skills_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "soft_skills",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    soft_skill_name = table.Column<string>(type: "text", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soft_skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_soft_skills_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hard_skills_resume_id",
                table: "hard_skills",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "IX_soft_skills_resume_id",
                table: "soft_skills",
                column: "resume_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hard_skills");

            migrationBuilder.DropTable(
                name: "soft_skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_resumes",
                table: "resumes");

            migrationBuilder.RenameTable(
                name: "resumes",
                newName: "users");

            migrationBuilder.AddColumn<string>(
                name: "hard_skills",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "soft_skills",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_id",
                table: "users",
                column: "user_id",
                unique: true);
        }
    }
}
