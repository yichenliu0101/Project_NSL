using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Interfaces;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
	public class BackMembersRepository : IMemberRepo
	{
		private NSL_DBContext _db;

		public BackMembersRepository(NSL_DBContext db)
		{
			_db = db;
		}

		public async Task<string> Delete(int id)
		{
			var member = await _db.Members.FindAsync(id);
			if (member == null)
			{
				throw new Exception("Cannot Find Data");
			}

			try
			{
				_db.Members.Remove(member);
				await _db.SaveChangesAsync();
				return "Success";
			}
			catch(Exception ex)
			{
				return $"Failed, Message = {ex.Message}";
			}
		}

		public Task<List<MemberListItemDto>> GetList(string? search, params int[]? displayRole)
		{
			#region Query
			var query = from m in _db.Members
					   join c in _db.Citys 
							on m.CityId equals c.Id into jt1
					   from jis1 in jt1.DefaultIfEmpty()
					   join a in _db.Areas 
							on m.AreaId equals a.Id into jt2
					   from jis2 in jt2.DefaultIfEmpty()
					   join r in _db.Roles 
							on m.Role equals r.Id into jt3
					   from jis3 in jt3.DefaultIfEmpty()
						where  m.Role != 4
					   select new MemberListItemDto
					   {
						   MemberId = m.Id,
						   Name = m.Name,
						   Gender = (m.Gender == null) ? "" : ((bool)m.Gender ? "男" : "女"),
						   Birthday = m.Birthday,
						   Email = (m.Email==null) ? "" : m.Email,
						   CityName = (jis1.Name == null) ? "" : jis1.Name,
						   AreaName = (jis2.Name == null) ? "" : jis2.Name,
						   ImageName = (m.ImageName == null) ? "default.jpg" : m.ImageName,
						   Role = new Roles() { Id = jis3.Id, Name = jis3.Name},
					   };
			#endregion

			//判斷是否有條件
			if (!string.IsNullOrEmpty(search) )
			{
				query = query.Where(x=>x.Name.Contains(search));
			}
			if(displayRole != null &&displayRole.Count() >0)
			{
				switch (displayRole.Count())
				{
					case 1:
						query = query.Where(x => x.Role.Id == displayRole[0]);
						break;
					case 2:
						query = query.Where(x => x.Role.Id == displayRole[0] || x.Role.Id == displayRole[1]);
						break;
					case 3:
						query = query.Where(x => x.Role.Id == displayRole[0] || x.Role.Id == displayRole[1] || x.Role.Id == displayRole[2]);
						break;
					default:
						break;
				}
			}

			return query.AsNoTracking().ToListAsync();
		}
	}
}
