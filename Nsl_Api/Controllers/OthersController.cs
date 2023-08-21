using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.DapperRepositories;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using NuGet.Protocol;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OthersController : ControllerBase
    {
        private readonly NSL_DBContext _db;
        private readonly IOthersRepo _repo;
		private readonly IConfiguration _configuration;

        public OthersController(IConfiguration configuration,NSL_DBContext db)
        {
			_db = db;
            _repo = new OthersRepositoriesEF(_db);
            _configuration= configuration;
        }

        [HttpGet]
        [Route("languages")]
        public async Task<ActionResult<IEnumerable<Languages>>> GetLanguages(int Id)
        {
            var list = new GetLangsData();
            try
            {
                list.Data = await _repo.GetLangs(Id);
                return list.Data;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("payment")]
        public async Task<ActionResult<IEnumerable<PaymentMethods>>> GetPayment(int paymentId)
        {
            var list = new GetPayData();
            try
            {
                list.Data = await _repo.GetPay(paymentId);
                return list.Data;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("Cities")]
        public async Task<ActionResult<IEnumerable<Citys>>> GetCities()
        {
            var list = new CityListData();
            try
            {
                list.Data = await _repo.GetCitys();
                return list.Data;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("Areas")]
        public async Task<ActionResult<IEnumerable<Areas>>> GetAreas(int cityId)
        {
            var list = new AreaListData();
            try
            {
                list.Data = await _repo.GetAreas(cityId);
                return list.Data;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

		[HttpGet]
		[Route("Categories")]
		public async Task<ActionResult<IEnumerable<Categories>>> GetCategories(int categoryId)
		{
			var list = new CategoryListData();
			try
			{
				list.Data = await _repo.GetCategories(categoryId);
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[HttpGet]
		[Route("Chat")]
		public async Task<ActionResult<IEnumerable<MessageListDto>>> GetChates(int senderid)
		{
			try
			{
				var messageList = new MessageListDto();
				var repo = new ChatsRepo(_configuration);
				var dto = await repo.GetMessage(_db,senderid);
				return dto;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[HttpGet]
        [Route("MessageDetail")]
        public async Task<ActionResult<IEnumerable<spMessageDetailResult>>> GetMessageDetail(int senderid, int chatMemberId)
        {
            try
            {
                var repo = new ChatsRepo(_configuration);
                var dto = await repo.GetMessageDetail(_db,senderid, chatMemberId);
                return dto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
		
        [HttpPost]
		[Route("GetTeacherLessonList")]
		public async Task<ActionResult<IEnumerable<TeacherLessonListDto>>> GetTeacherLessonList(lessonSearchDto dto,int? langId)
		{
			ITeacherLessonRepo repo = new GetTeacherLessonListEF(_db);
			if (_db.Members == null)
			{
				return NotFound();
			}
			var list = new TeacherLessonListData();
			try
            {
				var teacher = await repo.GetTeacherList(dto);
                foreach (var item in teacher)
                {
                    var tags = (from tg in _db.TeachersTags
                                join ot in _db.Tags on tg.TagId equals ot.Id
                                where tg.TeacherId == item.TeacherId
                                select ot.Name).ToList();

                    if (tags.Count > 0)
                    {
                        item.TagNames = new List<string>();
                        item.TagNames.AddRange(tags);
                    }
                }

				list.Data = teacher;
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return list.Data;
		}

		[HttpGet]
		[Route("BankCode")]
		public async Task<ActionResult<IEnumerable<BankCode>>> GetBankCde(int bankCodeId)
		{
			var list = new BankCodeListData();
			try
			{
				list.Data = await _repo.GetBankCode(bankCodeId);
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpGet]
        [Route("Tags")]
        public async Task<ActionResult<IEnumerable<Tags>>> GetTags()
        {
            var list = new TagsListData();
            try
            {
                list.Data = await _repo.GetTags();
                return list.Data;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

		[HttpPost]
		[Route("CreateTags")]
		public Task<ResultDto> CreateTags (OtherTags tags)
		{
			var result = new ResultDto() { isSuccess = true };
			if (tags == null)
			{
				result.isSuccess = false;
				result.errMsg = "資料未傳送進來";
				return Task.FromResult(result);
			}

			try
			{
				_repo.CreateTags(tags);
			}

			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}


			[HttpGet]
		[Route("GetApplyDatas")]
		public async Task<ActionResult<TeacherApplyDatas>> GetApplyDatas()
		{
			var datas = new TeacherApplyDatas();
			try
			{
				datas = await _repo.GetApplyDatas();
				return datas;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("TutorPeriod")]
		public async Task<ActionResult<IEnumerable<TutorPeriod>>> TutorPeriod()
		{
			var list = new TutorPeriodListData();
			try
			{
				list.Data = await _repo.GetTutorPeriod();
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[HttpGet]
		[Route("GetDeveloper")]
		public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetDevelopers()
		{
			var list = new DeveloperListData();
			try
			{
				list.Data = await _repo.GetDevelopers();
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}


