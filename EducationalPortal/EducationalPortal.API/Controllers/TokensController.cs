﻿using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    public class TokensController : ApiControllerBase
    {
        private readonly ITokensService _tokenService;

        public TokensController(ITokensService tokenService)
        {
            this._tokenService = tokenService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokensModel>> Refresh([FromBody] TokensModel tokensModel)
        {
            return await this._tokenService.Refresh(tokensModel, Email);
        }
    }
}
