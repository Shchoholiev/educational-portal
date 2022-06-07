using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EducationalPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : Controller
    {
        protected string? Email => User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        protected void SetPagingMetadata(IPagedList pagedList)
        {
            var metadata = new
            {
                pagedList.PageSize,
                pagedList.PageNumber,
                pagedList.TotalPages,
                pagedList.HasNextPage,
                pagedList.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        }
    }
}
