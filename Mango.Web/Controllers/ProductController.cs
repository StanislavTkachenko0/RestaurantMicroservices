using Mango.Web.Models;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetAllProductsAsync<ResponseDto>(accesToken);
            
            if (response is ResponseDto responseDto && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accesToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.CreateProductAsync<ResponseDto>(model, accesToken);

                if (response is ResponseDto responseDto && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accesToken);

            if (response is ResponseDto responseDto && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accesToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.UpdateProductAsync<ResponseDto>(model, accesToken);

                if (response is ResponseDto responseDto && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accesToken);

            if (response is ResponseDto responseDto && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            if (model.ProductId != null)
            {
                var accesToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.DeleteProductAsync<ResponseDto>(model.ProductId, accesToken);

                if (response is ResponseDto responseDto && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }
    }
}
