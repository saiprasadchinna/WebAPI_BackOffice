using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI_BackOffice.Models;
using MauiAuth0App.Auth0;
using System.Net.Http;
using IdentityModel.OidcClient.Results;
using IdentityModel.OidcClient;

namespace WebAPI_BackOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private const string publicMessage = "The API doesn't require an access token to share this message.";
        private const string protectedMessage = "The API successfully validated your access token.";
        private const string adminMessage = "The API successfully recognized you as an admin.";

        private readonly Auth0Client auth0Client;

        public MessagesController(Auth0Client client)
        {
            auth0Client = client;
        }

        [HttpGet("public")]
        public ApiResponse GetPublicMessage()
        {
            return new ApiResponse(publicMessage);
        }

        [HttpGet("protected")]
        [Authorize]
        public ApiResponse GetProtectedMessage()
        {
            return new ApiResponse(protectedMessage);
        }

        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public ApiResponse GetAdminMessage()
        {
            return new ApiResponse(adminMessage);
        }

        [HttpGet("defaultOktaTokenValid")]
        public async Task<JsonResult> DefaultValid(string accessToken)
        {
            var results = await getUserInfo(accessToken);
            return new JsonResult(results);
        }

        private async Task<UserInfoResult> getUserInfo(string? accessToken)
        {
            var results = await auth0Client.getUserInfo(accessToken);
            return results;
        }
    }
}
