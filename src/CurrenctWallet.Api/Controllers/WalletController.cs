using CurrencyWallet.Core.Abstractions;
using CurrencyWallet.Core.Exceptions;
using CurrencyWallet.DTO;
using CurrencyWallet.DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrenctWallet.Api.Controllers
{
    [ApiController]
    [Route("wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;

        public WalletController(IWalletService _walletService)
        {
            if (_walletService == null) throw new ArgumentNullException(nameof(_walletService));
            walletService = _walletService;
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetCurrencyWallets()
        {
            var wallets = await walletService.GellAllWallets();
            
            if (wallets is not null && wallets.Count() > 0)
            {
                return Ok(wallets);
            }

            return NotFound();
        }
            
        [HttpPost]
        public async Task<ActionResult> CreateWallet(string Name)
        {
            try
            {
                await walletService.CreateWallet(new Wallet { Name = Name });
                return StatusCode(StatusCodes.Status201Created); ;
            }
            catch (WalletExistException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("{walletId}/deposite")]
        public async Task<ActionResult> DepositFund(int walletId, [FromBody] Transfer deposite)
        {
            try
            {
                await walletService.DepositeFunds(walletId, deposite);
                return NoContent(); ;
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{walletId}/withdraw")]
        public async Task<ActionResult> WithdrawFund(int walletId, [FromBody] Transfer deposite)
        {
            try
            {
                await walletService.WithdrawFunds(walletId, deposite);
                return NoContent(); ;
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("{walletId}/exchange")]
        public async Task<ActionResult> ExchangeFund(int walletId, [FromBody] Exchange exchange)
        {
            try
            {
                await walletService.ExchangeFund(walletId, exchange);
                return NoContent(); ;
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
