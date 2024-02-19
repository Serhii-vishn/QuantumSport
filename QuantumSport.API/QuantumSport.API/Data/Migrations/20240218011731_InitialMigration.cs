using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantumSport.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coach",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Achievement = table.Column<string>(type: "nvarchar(555)", maxLength: 555, nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nutritionist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Spezialization = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutritionist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SportSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(555)", maxLength: 555, nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Phone = table.Column<string>(type: "nchar(13)", fixedLength: true, maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoachSportSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoachId = table.Column<int>(type: "int", nullable: false),
                    SportSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachSportSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachSportSection_Coach_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoachSportSection_SportSection_SportSectionId",
                        column: x => x.SportSectionId,
                        principalTable: "SportSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndividualMealPlanOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NutritionistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualMealPlanOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualMealPlanOrder_Nutritionist_NutritionistId",
                        column: x => x.NutritionistId,
                        principalTable: "Nutritionist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndividualMealPlanOrder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndividualTrainingProgramOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CoachId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualTrainingProgramOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualTrainingProgramOrder_Coach_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndividualTrainingProgramOrder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxAmount = table.Column<int>(type: "int", nullable: false),
                    ActualAmount = table.Column<int>(type: "int", nullable: false),
                    CoachSportSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_CoachSportSection_CoachSportSectionId",
                        column: x => x.CoachSportSectionId,
                        principalTable: "CoachSportSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainingRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TrainingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainingRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTrainingRecord_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTrainingRecord_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachSportSection_CoachId_SportSectionId",
                table: "CoachSportSection",
                columns: new[] { "CoachId", "SportSectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoachSportSection_SportSectionId",
                table: "CoachSportSection",
                column: "SportSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualMealPlanOrder_NutritionistId",
                table: "IndividualMealPlanOrder",
                column: "NutritionistId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualMealPlanOrder_UserId_NutritionistId",
                table: "IndividualMealPlanOrder",
                columns: new[] { "UserId", "NutritionistId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndividualTrainingProgramOrder_CoachId",
                table: "IndividualTrainingProgramOrder",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualTrainingProgramOrder_UserId_CoachId",
                table: "IndividualTrainingProgramOrder",
                columns: new[] { "UserId", "CoachId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Training_CoachSportSectionId_StartDate",
                table: "Training",
                columns: new[] { "CoachSportSectionId", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Phone",
                table: "User",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingRecord_TrainingId",
                table: "UserTrainingRecord",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingRecord_UserId_TrainingDate",
                table: "UserTrainingRecord",
                columns: new[] { "UserId", "TrainingDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndividualMealPlanOrder");

            migrationBuilder.DropTable(
                name: "IndividualTrainingProgramOrder");

            migrationBuilder.DropTable(
                name: "UserTrainingRecord");

            migrationBuilder.DropTable(
                name: "Nutritionist");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "CoachSportSection");

            migrationBuilder.DropTable(
                name: "Coach");

            migrationBuilder.DropTable(
                name: "SportSection");
        }
    }
}
