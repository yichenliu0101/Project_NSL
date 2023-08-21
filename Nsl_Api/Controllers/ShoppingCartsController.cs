using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.DapperRepositories;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;

namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly NSL_DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly OthersRepositoriesEF _repo;

        public ShoppingCartsController(NSL_DBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _repo = new OthersRepositoriesEF(_context);
        }

        [HttpGet]
        [Route("FindShoppingCart")]
        public async Task<ActionResult<IEnumerable<ShowShoppingCart>>> GetShoppingCart(int memberId)
        {

            if (_context.ShoppingCart.Where(x => x.MemberId == memberId) == null)
            {
                return NotFound();
            }
            var query = (from ms in _context.ShoppingCart
                         join tt in _context.TeachersResume on ms.TeacherId equals tt.TeacherId
                         join t in _context.TeacherId on tt.TeacherId equals t.Id
                         join mm in _context.Members on t.MemberId equals mm.Id
                         where ms.MemberId == memberId && ms.Status==false

                         select new ShowShoppingCart
                         {
                             Id = ms.Id,
                             MemberId = ms.MemberId,
                             TeacherId = tt.TeacherId,
                             ImageName = mm.ImageName,
                             Title = tt.Title,
                             Price = tt.Price,
                             Count = ms.Count,
                         });
            return await query.ToListAsync();
        }

        [HttpPost]
        [Route("CreateShoppingCart")]
        public ResultDto ShoppingCarCreate(ShowShoppingCart dto)
        {

            ResultDto result = new ResultDto();
            result.isSuccess = true;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var item = _context.ShoppingCart.Where(o => o.MemberId == dto.MemberId && o.TeacherId == dto.TeacherId && o.Status == false).FirstOrDefault();
                    //查詢資料庫中符合的購物車紀錄(條件)

                    //Update--如果有找到資料 就做更新(數量往上遞加)
                    if (item != null)
                    {
                        item.Count++;
                    }
                    //Insert--如果沒有找到資料 就直接加入資料庫(數量為一筆)
                    else
                    {
                        ShoppingCart items = new ShoppingCart()
                        {
                            MemberId = dto.MemberId,
                            TeacherId = dto.TeacherId,
                            Count = 1,
                            Status = false
                        };

                        _context.ShoppingCart.Add(items); //存入以上東西到名為ShoppingCart的資料表
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result.isSuccess = false;
                    result.errMsg = ex.Message;
                    transaction.Rollback();
                }

                return result;
            }
        }


        [HttpPost]
        [Route("ShoppingCarUpdate")]
        public ResultDto ShoppingCarUpdate(int id, int qty)
        {
            ResultDto result = new ResultDto();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var cartitem = _context.ShoppingCart.FirstOrDefault(o => o.Id == id && o.Status == false);//從資料庫查詢未結帳的購物車紀錄
                    cartitem.Count = qty;//更改ShoppingCart資料表中的數量
                    cartitem.ModifiedTime = DateTime.Now;

                    _context.SaveChanges();
                    result.isSuccess = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result.isSuccess = false;
                    result.errMsg = ex.Message;
                    transaction.Rollback();
                }
                return result;

            }
        }



        [HttpGet]
        [Route("AllShopList")]
        public async Task<ActionResult<IEnumerable<GetAllShopListDto>>> GetAllShopList(string ordercode)
        {
            
            var repo = new ShoppingCarListDappers(_configuration);
            if (_context.ShoppingCart == null)
            {
                return NotFound();
            }
            var list = new AllShopListData();
            try
            {
                list.Data = await repo.GetAllShopList(ordercode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }



        [HttpPost]
        [Route("OrderSave")]
        public ConsumptionRecords OrderSave(ConsumptionRecords dto) 
        {
          
            string formmat = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);//取得亂數(訂單編號生成)
            using (var transaction = _context.Database.BeginTransaction())
            {
            try
                {  
                    
                    foreach(var item in dto.ShoppingCarId)
                    {
                        var shoppingdetail = _context.ShoppingCart.Find(item);
                        if (shoppingdetail == null) throw new Exception("找不到此購物車資料");
                        shoppingdetail.Status = true;  //付款狀態改成true(已付款)
                        _context.SaveChanges();
                    }

                    //存入資料庫購物車主檔
                    MembersConsumptionRecords record = new MembersConsumptionRecords()
                    {
                        MemberId = dto.MemberId,
                        OrderCode = formmat,
                        PaymentId = dto.PaymentId,
                        CouponId = dto.CouponId,
                        Status = false,
                    };
                    _context.MembersConsumptionRecords.Add(record);
                    _context.SaveChanges();

                    if (dto.CouponId!=null)
                    {//存入折價券歷史清單資料表
                        CouponUsageHistory useingCoupon = new CouponUsageHistory()
                        {
                            MemberId = dto.MemberId,
                            CouponId = (int)dto.CouponId,
                        };

                        _context.CouponUsageHistory.Add(useingCoupon);
                        _context.SaveChanges();
                    }

                    //存入資料庫購物車明細檔
                    foreach (ConsumptionRecordsDetail item in dto.Detail)// 因為是集合需要用foreach一一抓出來
                    {
                        MembersConsumptionRecordDetails recorddetail = new MembersConsumptionRecordDetails()
                        {
                            MembersConsumptionRecordId = record.Id,
                            TeacherId = item.TeacherId,
                            Count = item.Count,
                            CurrentPrice = item.CurrentPrice,

                        };
                        _context.MembersConsumptionRecordDetails.Add(recorddetail);

                        //存入課程數資料表
                        //資料庫有相同課程時課堂數往上累加

                        var query = _context.MembersTutorStock.Where(o => o.MemberId == dto.MemberId && o.TeacherId==item.TeacherId).FirstOrDefault();
                        //尋找資料庫是否有相同課程的資料 用memberid 跟 teacherid的條件找到一筆

                        if (query==null)
                        {
                            MembersTutorStock TutorStock = new MembersTutorStock()
                            {
                                MemberId = dto.MemberId,
                                TeacherId = item.TeacherId,
                                TutorStock = item.Count
                            };
                            _context.MembersTutorStock.Add(TutorStock);
                        }
                        else
                        {
                            query.TutorStock = query.TutorStock+item.Count;

                        }
                        
                        _context.SaveChanges();
                    }
                    
                    dto.OrderCode = formmat;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                   
                    transaction.Rollback();
                }

                return dto;
            }
        }



        [HttpPost]
        [Route("OrderListUpdate")]
        public ResultDto OrderListUpdate(string ordercode) 
        {
            var result = new ResultDto() { isSuccess = true};
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var orderlist = _context.MembersConsumptionRecords.FirstOrDefault(o => o.OrderCode== ordercode && o.Status == false);
                    orderlist.Status = true;
                    _context.SaveChanges();
                    
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result.isSuccess = false;
                    result.errMsg = ex.Message;
                    transaction.Rollback();
                }
                return result;
            }

        }





        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCart(int id, ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return BadRequest();
            }

            var updateShoppingCart = _context.ShoppingCart.Find(id);
            updateShoppingCart.Count++;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       


        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> PostShoppingCart(ShoppingCart shoppingCart)
        {
            if (_context.ShoppingCart == null)
            {
                return Problem("Entity set 'NSL_DBContext.ShoppingCart'  is null.");
            }
            _context.ShoppingCart.Add(shoppingCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCart", new { id = shoppingCart.Id }, shoppingCart);
        }

        // DELETE: api/ShoppingCarts/5

        [HttpDelete]
        [Route("DeleteShop")]
        public Task<ResultDto> DeleteShoppingCart(int Id)
        {
            var result = new ResultDto() { isSuccess = true };
            try
            {
                _repo.DeleteShoppingCar(Id);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.errMsg = ex.Message;
            }
            return Task.FromResult(result);


        }

        private bool ShoppingCartExists(int id)
        {
            return (_context.ShoppingCart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
