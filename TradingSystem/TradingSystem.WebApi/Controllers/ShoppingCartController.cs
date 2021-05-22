﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

using TradingSystem.Business.Market;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShoppingCartController : ControllerBase
    {
        public ShoppingCartController(MarketShoppingCartService marketShoppingCartService)
        {
            MarketShoppingCartService = marketShoppingCartService;
        }

        public MarketShoppingCartService MarketShoppingCartService { get; }

        [HttpGet]
        public ActionResult<ShoppingCartDTO> MyShoppingCart([FromBody] UsernameDTO usernameDTO)
        {
            if (string.IsNullOrWhiteSpace(usernameDTO.Username))
            {
                return BadRequest("Missing parameter values");
            }

            Dictionary<Guid, Dictionary<ProductData, int>>? result = MarketShoppingCartService.ViewShoppingCart(usernameDTO.Username);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(ShoppingCartDTO.FromDictionary(result));
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ShoppingCartAddProductDTO addProductDTO)
        {
            Result<Dictionary<Guid, Dictionary<ProductData, int>>>? result = await MarketShoppingCartService.EditShoppingCart
            (
                addProductDTO.Username,
                new List<Guid>(),
                new Dictionary<Guid, int> { [addProductDTO.ProductId] = addProductDTO.Quantity },
                new Dictionary<Guid, int>()
            );

            if (result == null || (result.IsErr && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr)
            {
                return InternalServerError(result.Mess);
            }

            return Ok();
        }
    }
}