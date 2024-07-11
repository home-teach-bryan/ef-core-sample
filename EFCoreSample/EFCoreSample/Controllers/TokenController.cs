﻿using System.Security.Claims;
using EFCoreSample.Jwt;
using EFCoreSample.Models.Request;
using EFCoreSample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Controllers;

/// <summary>
/// 取得 Token
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public TokenController(IUserService userService, JwtTokenGenerator jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    /// <summary>
    /// 取得Token
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Token</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public IActionResult GetToken([FromBody] GetTokenRequest request)
    {
        var (isValid, user) = _userService.IsValid(request.Name, request.Password);
        if (!isValid)
        {
            return BadRequest("帳號或密碼錯誤");
        }
        var token = _jwtTokenGenerator.GenerateJwtToken(user.Id, user.Name, user.Roles);
        return Ok(token);
    }

    /// <summary>
    /// 取得角色
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("Roles")]
    public IActionResult GetRoles()
    {
        var roleClaim = base.HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.Role);
        return Ok(roleClaim.Select(item => item.Value));
    }
}