using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeGenerator.ApiService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resume_name",
                table: "resumes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resume_name",
                table: "resumes");
        }
    }
}
