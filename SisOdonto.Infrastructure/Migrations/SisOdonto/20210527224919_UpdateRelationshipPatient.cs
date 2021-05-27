using Microsoft.EntityFrameworkCore.Migrations;

namespace SisOdonto.Infrastructure.Migrations.SisOdonto
{
    public partial class UpdateRelationshipPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_HealthInsuranceId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_HealthInsuranceId",
                table: "Patients",
                column: "HealthInsuranceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_HealthInsuranceId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_HealthInsuranceId",
                table: "Patients",
                column: "HealthInsuranceId",
                unique: true,
                filter: "[HealthInsuranceId] IS NOT NULL");
        }
    }
}
