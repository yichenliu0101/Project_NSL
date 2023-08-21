using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;

namespace Nsl_Api.Models.Interfaces
{
	public interface IOthersRepo
	{
		Task<List<Languages>> GetLangs(int Id);
        Task<List<PaymentMethods>>GetPay(int paymentId);
        Task<List<Citys>> GetCitys();
		Task<List<Areas>> GetAreas(int cityId);
		Task<List<Categories>> GetCategories(int categoryId);
		Task<List<BankCode>> GetBankCode(int bankCodeId);
        Task<List<Tags>> GetTags();
		string CreateTags(OtherTags tags);
		Task<TeacherApplyDatas> GetApplyDatas();
		Task<List<TutorPeriod>> GetTutorPeriod();
		Task<List<DeveloperDto>> GetDevelopers();
    }
}
