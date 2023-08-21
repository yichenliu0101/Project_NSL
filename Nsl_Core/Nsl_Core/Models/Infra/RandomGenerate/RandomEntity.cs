using Nsl_Core.Models.EFModels;
using XAct.Library.Settings;

namespace Nsl_Core.Models.Infra.RandomGenerate
{
    public class RandomEntity
    {
        private readonly RandomGenerator _generator;
        private readonly NSL_DBContext _db;
        private readonly IConfiguration _configuration;
        public RandomEntity(NSL_DBContext db, IConfiguration configuration)
        {
            _db = db; 
            _configuration = configuration;
            _generator = new RandomGenerator(_db, _configuration);
        }
        public void CreateMember()
        {

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 1; i < 200; i++)
                    {
                        int length = 12;
                        string password = _generator.GenerateRandomPassword(length, out string hashPassword, out string salt);
                        var member = new Members
                        {

                            Name = _generator.RandomName(),
                            Birthday = _generator.RandomBirthDate(),
                            Phone = _generator.RandomPhone(),
                            Email = _generator.RandomEmail(),
                            Password = password,
                            EncryptedPassword = hashPassword,
                            Salt = salt,
                            CityId = _generator.RandomCityId(out int areaId),
                            AreaId = areaId,
                            Gender = _generator.RandomBool(),
                            EmailCheck = _generator.RandomBool(),
                            EmailToken = Guid.NewGuid().GetHashCode().ToString(),
                            Role = 1,
                        };
                        _db.Members.Add(member);
                        _db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }
    }
}
