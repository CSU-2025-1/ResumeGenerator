using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeGenerator.ApiService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeCreatingRetriesCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "retry_count",
                table: "resumes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "retry_count",
                table: "resumes");
        }
    }
}
