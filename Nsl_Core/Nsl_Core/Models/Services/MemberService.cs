using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.Infra.Repositories.ADORepositories;
using Nsl_Core.Models.Interfaces;

namespace Nsl_Core.Models.Services
{
    public class MemberService
    {
        //public int Create(MemberDto dto)
        //{
        //    //todo 驗證分類名稱是否重複

        //    IMemberRepo repo = new MemberRepository();
            
        //    var dtoInDb = repo.GetByEmail(dto.Email);

        //    if (dtoInDb != null)
        //    {
        //        throw new Exception("抱歉,此分類名稱已存在,無法新增!!");
        //    }
        //    //建立一筆新紀錄
        //    int newId = repo.Create(dto);

        //    return newId;
        //}
        //public int Update(MemberDto dto)
        //{
        //    //todo 驗證分類名稱是否重複

        //    IMemberRepo repo = new MemberRepository();
        //    var dtoInDb = repo.GetByEmail(dto.Email);

        //    if (dtoInDb != null && dtoInDb.Id != dto.Id)
        //    {
        //        throw new Exception("抱歉,此分類名稱已存在,無法更新!!");
        //    }
        //    //更新紀錄
        //    int rows = repo.Update(dto);

        //    return rows;
        //}
    }
}
