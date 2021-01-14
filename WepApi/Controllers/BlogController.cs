using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebApi.Utility;
using WepApi.DataAccessLayer;
using WepApi.Models;

namespace WepApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        SqlDataBlock sql = new SqlDataBlock();

        [HttpGet("Get")]
        public IEnumerable<Blog> Get([FromBody]  SearchFilter SearchFilter)
        {
            try
            {
                DataTable dt = sql.ExecuteDataTable(CommandType.StoredProcedure, "sp_BlogGet");

                var result = (from rw in dt.Select()
                              select new Blog
                              {
                                  Id = Convert.ToInt32(rw["Id"]),
                                  BlogDescription = Convert.ToString(rw["BlogDescription"]),
                                  UserName = Convert.ToString(rw["UserName"]),
                                  InstertDate = Convert.ToDateTime(rw["InstertDate"])
                              }).ToList();

                if (!string.IsNullOrEmpty(SearchFilter.searchString))
                {
                    result = result.Where(s => s.BlogDescription.Contains(SearchFilter.searchString)
                                           || s.UserName.Contains(SearchFilter.searchString)).ToList();
                }

                switch (SearchFilter.sortOrder)
                {
                    case "name_desc":
                        result = result.OrderByDescending(s => s.UserName).ToList();
                        break;
                    case "Date":
                        result = result.OrderBy(s => s.InstertDate).ToList();
                        break;
                    case "date_desc":
                        result = result.OrderByDescending(s => s.InstertDate).ToList();
                        break;
                    default:
                        result = result.OrderBy(s => s.UserName).ToList();
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return null;
            }
        }

        [HttpGet("GetbyId/{id}")]
        public ActionResult<Blog> GetbyId(int id)
        {
            try
            {
                DataTable dt = sql.ExecuteDataTable(CommandType.StoredProcedure, "sp_BlogGetbyId", new SqlParameter("@Id", id));

                var result = new Blog
                {
                    Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                    BlogDescription = Convert.ToString(dt.Rows[0]["BlogDescription"]),
                    UserName = Convert.ToString(dt.Rows[0]["UserName"]),
                    InstertDate = Convert.ToDateTime(dt.Rows[0]["InstertDate"])
                };

                return result;
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return null;
            }
        }

        [HttpPost("Insert")]
        public ActionResult Insert(Blog blog)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_BlogInsert",
                    new SqlParameter("@BlogDescription", blog.BlogDescription),
                    new SqlParameter("@UserName", blog.UserName)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("Update")]
        public ActionResult Update(Blog blog)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_BlogUpdate",
                    new SqlParameter("@BlogDescription", blog.BlogDescription),
                    new SqlParameter("@UserName", blog.UserName)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_BlogDelete",
                    new SqlParameter("@Id", id)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }
    }
}