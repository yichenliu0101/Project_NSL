using Humanizer;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.ViewModels;
using Nsl_Api.Models.Dtos;
using Nsl_Api.Models.Interfaces;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
	public class TeacherRepositoryEF : ITeacherRepo
	{
		private NSL_DBContext _db;

		public TeacherRepositoryEF(NSL_DBContext db)
		{
			_db = db;
		}

        public string DeleteApply(int teacherId)
        {
            var teacher = _db.TeachersApply.Find(teacherId);
            if (teacher == null)
            {
                throw new Exception("Cannot Find Data");
            }

            try
            {
                _db.TeachersApply.Remove(teacher);
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed, Message = {ex.Message}";
            }
        }

        public string DeleteComment(int id)
        {
            var teacher = _db.Comments.Find(id);
            if (teacher == null)
            {
                throw new Exception("Cannot Find Data");
            }

            try
            {
                _db.Comments.Remove(teacher);
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed, Message = {ex.Message}";
            }
        }

		public string DeleteResume(int TeacherId)
		{
			var teacher = _db.TeachersResume.Find(TeacherId);

			if (teacher == null)
			{
				throw new Exception("Cannot Find Data");
			}

			try
			{
				_db.TeachersResume.Remove(teacher);
				_db.SaveChanges();
				return "Success";
			}
			catch (Exception ex)
			{
				return $"Failed, Message = {ex.Message}";
			}
		}

		//public string DeleteResume(int TeacherId)
		//{
		//	using (var transaction = _db.Database.BeginTransaction())
		//	{
		//		try
		//		{
		//			var teacher = _db.TeachersResume.Find(TeacherId);
		//			if (teacher == null)
		//			{
		//				throw new Exception("查無此筆資料");
		//			}

		//			var relatedApplies = _db.TeachersApply.Where(a => a.TeacherId == TeacherId);
		//			foreach (var apply in relatedApplies)
		//			{
		//				var relatedTeacherIds = _db.TeacherId.Where(t => t.Id == apply.TeacherId);
		//				_db.TeacherId.RemoveRange(relatedTeacherIds);

		//				var teacherIdEntry = _db.TeacherId.Find(apply.TeacherId);
		//				if (teacherIdEntry != null && teacherIdEntry.MemberId != null)
		//				{
		//					var member = _db.Members.Find(teacherIdEntry.MemberId);
		//					if (member != null && member.Role == 2)
		//					{
		//						member.Role = 1;
		//					}
		//				}

		//				_db.TeachersApply.Remove(apply);
		//			}

		//			_db.TeachersResume.Remove(teacher);

		//			_db.SaveChanges();

		//			transaction.Commit();

		//			return "Success";
		//		}
		//		catch (Exception ex)
		//		{
		//			transaction.Rollback();
		//			return $"Failed, Message = {ex.Message}";
		//		}
		//	}
		//}


		public async Task<string> Update(int id)
		{
			var teacher = await _db.TeachersApply.FindAsync(id);
			if (teacher == null)
			{
				throw new Exception("Cannot Find Data");
			}

			try
			{
				var member = from t in _db.TeacherId
							 join m in _db.Members on t.MemberId equals m.Id
							 select new { m.Role };
				var memberId = await _db.TeacherId.FindAsync(id);

				var teacherMemberData = await _db.Members.FindAsync(memberId.MemberId);

				teacherMemberData.Role = 2;


				await _db.SaveChangesAsync();
				return "Success";
			}
			catch (Exception ex)
			{
				return $"Failed, Message = {ex.Message}";
			}
		}
		public Task<List<TeacherApplyListDto>> GetApplyList(int teacherId)
		{
			try
			{
				var teachersApply = from ta in _db.TeachersApply
									join t in _db.TeacherId on ta.TeacherId equals t.Id
									join m in _db.Members on t.MemberId equals m.Id
									join c in _db.Categories on ta.CategoryId equals c.Id
									where t.Id == teacherId
									select new TeacherApplyListDto
									{
										Id = teacherId,
										CategoryName = c.Name,
										StatusName = (m.Role == 2) ? "已審核" : "未審核",
										TeacherName = m.Name,
										ImageName = m.ImageName,
									};
				return teachersApply.ToListAsync();
			}
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

		public Task<List<TeacherApplyListDto>> GetAllApplyList(string? search, int? searchLan)
		{
			try
			{
				var teachersApply = from ta in _db.TeachersApply
									join t in _db.TeacherId on ta.TeacherId equals t.Id
									join m in _db.Members on t.MemberId equals m.Id
									join c in _db.Categories on ta.CategoryId equals c.Id
									orderby ta.ApplyTime descending
									select new TeacherApplyListDto
									{
										Id = t.Id,
										CategoryId = c.Id,
										CategoryName = c.Name,
										StatusName = (m.Role == 2) ? "已審核" : "未審核",
										TeacherName = m.Name,
										ImageName = m.ImageName,
										Intro = ta.Intro,
									};
				if (!string.IsNullOrEmpty(search))
				{
					teachersApply = teachersApply.Where(x => x.TeacherName.Contains(search));
				}

				if (searchLan != null)
				{
					teachersApply = teachersApply.Where(x => x.CategoryId == searchLan);
				}

				return teachersApply.ToListAsync();
			}
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

		public Task<List<TeacherTagsDto>> GetTeacherTags(int teacherId)
		{
			try
			{
				var teacherstags = from tt in _db.TeachersTags
								   join t in _db.Tags on tt.TagId equals t.Id
								   where tt.TeacherId == teacherId
								   orderby tt.CreatedTime
								   select new TeacherTagsDto
								   {
									   TeacherId = teacherId,
									   TagsId = t.Id,
									   TagsName = t.Name,
								   };
				return teacherstags.ToListAsync();
			}
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

		public int CreateTeacherTags(TeacherCreate dto)
		{
			try
			{
				var tagInDb = _db.Tags.Where(x => x.Name == dto.TagsName).FirstOrDefault();
				if (tagInDb != null)
				{
					var tags = new TeachersTags()
					{
						TeacherId = dto.TeacherId,
						TagId = tagInDb.Id,
					};
					_db.TeachersTags.Add(tags);
					_db.SaveChanges();

					return tags.TeacherId;
				}
				else
				{
					var newTag = new Tags()
					{
						Name = dto.TagsName,
						CreatedTime = DateTime.Now,
					};

					_db.Tags.Add(newTag);
					_db.SaveChanges();

					dto.TagsId = newTag.Id;

					var tags = new TeachersTags()
					{
						TeacherId = dto.TeacherId,
						TagId = dto.TagsId,
					};
					_db.TeachersTags.Add(tags);
					_db.SaveChanges();

					return tags.TeacherId;
				}
			}
			catch (Exception)

			{
				throw new Exception("資料錯誤");
			}
        }

		public int EditTeacherTags(TeacherTagsDto dto)
		{
			var teachertags = _db.TeachersTags.Where(x => x.TeacherId == dto.TeacherId).FirstOrDefault();
			if (teachertags == null)
			{
				return 0;
			}

			try
			{
				teachertags.TagId = dto.TagsId;

				return _db.SaveChanges();
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public decimal? EditTeacherPrice(TeacherPriceVM vm)
		{
			var teacher = _db.TeachersResume.Where(x => x.TeacherId == vm.TeacherId).FirstOrDefault();
			if (teacher == null)
			{
				return null;
			}

			try
			{
				teacher.TeacherId = vm.TeacherId;
				teacher.Price = vm.Price;


				return _db.SaveChanges();
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public Task<List<Tags>> GetAutoCompleteTags(string input)
		{
			var searchTags = _db.Tags.Where(x => x.Name.StartsWith(input)).ToListAsync();

			return searchTags;
		}

        public string DeleteTeachersTags(TeachersTagsVM vm)
        {
			try
			{
				var teacherTagToDelete = _db.TeachersTags
					.FirstOrDefault(tt => tt.TeacherId == vm.TeacherId && tt.TagId == vm.TagId);

				if (teacherTagToDelete != null)
				{
					_db.TeachersTags.Remove(teacherTagToDelete);
					_db.SaveChanges();
				}

				return "Success";
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }
    }
}
