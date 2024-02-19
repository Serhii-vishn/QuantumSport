namespace QuantumSport.API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.SportSections.Any())
            {
                await context.SportSections.AddRangeAsync(GetSportSections());
                await context.SaveChangesAsync();
            }

            if (!context.Coaches.Any())
            {
                await context.Coaches.AddRangeAsync(GetCoaches());
                await context.SaveChangesAsync();
            }

            if (!context.Nutritionists.Any())
            {
                await context.Nutritionists.AddRangeAsync(GetNutritionists());
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(GetUsers());
                await context.SaveChangesAsync();
            }

            if (!context.CoachSportSections.Any())
            {
                await context.CoachSportSections.AddRangeAsync(GetCoachSportSections());
                await context.SaveChangesAsync();
            }

            if (!context.Trainings.Any())
            {
                await context.Trainings.AddRangeAsync(GetTrainings());
                await context.SaveChangesAsync();
            }

            if (!context.UserTrainingRecords.Any())
            {
                await context.UserTrainingRecords.AddRangeAsync(GetUserTrainingRecords());
                await context.SaveChangesAsync();
            }
        }

        private static IList<SportSectionEntity> GetSportSections()
        {
            return new List<SportSectionEntity>()
            {
                new SportSectionEntity()
                {
                    Name = "Бокс",
                    Description = "Контактний вид спорту, єдиноборство, " +
                                    "в якому спортсмени наносять один одному " +
                                    "удари кулаками в спеціальних рукавичках. " +
                                    "Боксер перемагає, якщо його суперник збитий з ніг " +
                                    "і не може піднятися протягом десяти секунд (нокаут), " +
                                    "або після отриманої травми.",
                    PictureFileName = "boxing.png",
                },
                new SportSectionEntity()
                {
                    Name = "Кросфіт",
                    Description = "Кроcфіт — молодий спортивний напрямок, " +
                                    "що стрімко розвивається та набирає популярність. " +
                                    "CrossFit — це новий погляд на спорт, " +
                                    "на здоровий спосіб життя і самого себе, " +
                                    "на свої можливості та прагнення.",
                    PictureFileName = "crossfit.png",
                },
                new SportSectionEntity()
                {
                    Name = "Фітнес",
                    Description = "Рух — це життя. Фітнес — це рух для сучасних людей, " +
                                    "які хочуть жити насичено та активно. " +
                                    "Cпорт може змінити на краще життя кожної людини, " +
                                    "головне — знайти напрямок, який буде вам до душі, " +
                                    "та людей, які підтримають ваше прагнення до здоров’я та краси.",
                    PictureFileName = "fitness.png",
                },
            };
        }

        private static IList<CoachEntity> GetCoaches()
        {
            return new List<CoachEntity>()
            {
                new CoachEntity()
                {
                    FirstName = "Анатолій",
                    LastName = "Ломаченко",
                    BirthDate = new DateTime(1964, 12, 14),
                    Education = "Одеський педагогічний інститут імені Ушинського",
                    Achievement = "Тренер і батько дворазового олімпійського чемпіона " +
                                    "і чемпіона світу Василя Ломаченка. " +
                                    "Заслужений тренер України (2006). " +
                                    "Грав провідну роль у підготовці національної збірної України з боксу. " +
                                    "Зокрема готував збірну до Чемпіонату світу з боксу 2011 року, " +
                                    "де Україна виборола першість, та до Олімпійських ігор 2012 року, " +
                                    "де збірна посіла друге місце.",
                    PictureFileName = "anatoly-lomachenko.jpg",
                },
                new CoachEntity()
                {
                    FirstName = "Максим",
                    LastName = "Лях",
                    BirthDate = new DateTime(1998, 4, 22),
                    Education = "Інститут фізичної культури та спорту",
                    Achievement = "Учасник змагань з Кросфіту. Учасник марафонських забігів. " +
                                    "Бронзовий призер аматорського байк-тріатлону. " +
                                    "Бронзовий призер забігу \"Осінь Донбасу\", дистанція 30км, 2017р. " +
                                    "Бронзовий призер напівмарафону «Пам'ять поколінь». " +
                                    "Срібний призер забігу \"Осінь Донбасу\" дистанція 30км, 2018р.",
                    PictureFileName = "maksym-ljah.jpg",
                },
                new CoachEntity()
                {
                    FirstName = "Ярослав",
                    LastName = "Сойников",
                    BirthDate = new DateTime(1985, 5, 5),
                    Education = "Інститут фізичного виховання і спорту. Вища.",
                    Achievement = "Сертифікований майстер-тренер Міжнародної Академії Life Fitness. " +
                                    "Єдиний в Україні майстер-тренер TRX (США) " +
                                    "сертифікований за всіма програмами навчання TRX " +
                                    "(вищий рівень кваліфікації TRX). " +
                                    "Майстер-тренер Hammer Strength. " +
                                    "Майстер-тренер Tigger Point Performance (США). " +
                                    "Майстер-тренер Procedos (Швеція). " +
                                    "Співзасновник та фітнес-директор мережі студій HiitWorks (Київ). " +
                                    "Чоловік року Фітнес індустрії України 2019 року (премія «Фітнес-трімуф 2019»).",
                    PictureFileName = "yaroslav-soynikov.png",
                },
            };
        }

        private static IList<NutritionistEntity> GetNutritionists()
        {
            return new List<NutritionistEntity>()
            {
                new NutritionistEntity()
                {
                    FirstName = "Генадій",
                    LastName = "Непийпиво",
                    BirthDate = new DateTime(1979, 7, 7),
                    Education = "Національний університет охорони здоров'я України імені П. Л. Шупика",
                    Spezialization = "Інтервальне голодування. Палеодієта. Кетодієта.",
                },
                new NutritionistEntity()
                {
                    FirstName = "Олена",
                    LastName = "Головач",
                    BirthDate = new DateTime(1990, 2, 3),
                    Education = "Університет Інженерії та Охорони Здоров'я",
                    Spezialization = "Вегетаріанська дієта. Дієта Дюкана.",
                },
            };
        }

        private static IList<UserEntity> GetUsers()
        {
            return new List<UserEntity>()
            {
                new UserEntity() { Name = "Billy Herrington", Phone = "+380994782390" },
                new UserEntity() { Name = "Arnold Schwarzenegger", Phone = "+380663232343" },
                new UserEntity() { Name = "Ronnie Coleman", Phone = "+380983454875" },
                new UserEntity() { Name = "Vasyl Virastyuk", Phone = "+380992301223" },
            };
        }

        private static IList<CoachSportSectionEntity> GetCoachSportSections()
        {
            return new List<CoachSportSectionEntity>()
            {
                new CoachSportSectionEntity()
                {
                    CoachId = 1,
                    SportSectionId = 1,
                },
                new CoachSportSectionEntity()
                {
                    CoachId = 2,
                    SportSectionId = 2,
                },
                new CoachSportSectionEntity()
                {
                    CoachId = 3,
                    SportSectionId = 3,
                },
            };
        }

        private static IList<TrainingEntity> GetTrainings()
        {
            return new List<TrainingEntity>()
            {
                new TrainingEntity()
                {
                    CoachSportSectionId = 1,
                    StartDate = new DateTime(2024, 4, 9, 14, 0, 0),
                    MaxAmount = 5,
                    ActualAmount = 2,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 1,
                    StartDate = new DateTime(2024, 4, 10, 18, 0, 0),
                    MaxAmount = 5,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 1,
                    StartDate = new DateTime(2024, 4, 11, 14, 0, 0),
                    MaxAmount = 5,
                    ActualAmount = 2,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 2,
                    StartDate = new DateTime(2024, 4, 8, 10, 0, 0),
                    MaxAmount = 10,
                    ActualAmount = 1,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 2,
                    StartDate = new DateTime(2024, 4, 9, 14, 0, 0),
                    MaxAmount = 10,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 2,
                    StartDate = new DateTime(2024, 4, 10, 10, 0, 0),
                    MaxAmount = 10,
                    ActualAmount = 1,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 2,
                    StartDate = new DateTime(2024, 4, 11, 14, 0, 0),
                    MaxAmount = 10,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 2,
                    StartDate = new DateTime(2024, 4, 12, 10, 0, 0),
                    MaxAmount = 10,
                    ActualAmount = 1,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 3,
                    StartDate = new DateTime(2024, 4, 8, 18, 0, 0),
                    MaxAmount = 4,
                    ActualAmount = 1,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 3,
                    StartDate = new DateTime(2024, 4, 10, 18, 0, 0),
                    MaxAmount = 4,
                    ActualAmount = 1,
                },
                new TrainingEntity()
                {
                    CoachSportSectionId = 3,
                    StartDate = new DateTime(2024, 4, 12, 18, 0, 0),
                    MaxAmount = 4,
                    ActualAmount = 1,
                },
            };
        }

        private static IList<UserTrainingRecordEntity> GetUserTrainingRecords()
        {
            return new List<UserTrainingRecordEntity>()
            {
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 9, 14, 0, 0),
                    UserId = 1,
                    TrainingId = 1,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 11, 14, 0, 0),
                    UserId = 1,
                    TrainingId = 3,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 8, 10, 0, 0),
                    UserId = 2,
                    TrainingId = 4,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 9, 14, 0, 0),
                    UserId = 2,
                    TrainingId = 1,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 10, 10, 0, 0),
                    UserId = 2,
                    TrainingId = 6,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 11, 14, 0, 0),
                    UserId = 2,
                    TrainingId = 3,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 12, 10, 0, 0),
                    UserId = 2,
                    TrainingId = 8,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 8, 18, 0, 0),
                    UserId = 4,
                    TrainingId = 9,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 10, 18, 0, 0),
                    UserId = 4,
                    TrainingId = 10,
                },
                new UserTrainingRecordEntity()
                {
                    TrainingDate = new DateTime(2024, 4, 12, 18, 0, 0),
                    UserId = 4,
                    TrainingId = 11,
                },
            };
        }
    }
}
