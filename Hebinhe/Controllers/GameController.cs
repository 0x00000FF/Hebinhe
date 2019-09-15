using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hebinhe.Models;

namespace Hebinhe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GameModel> Get()
        {
            using (var rng = RandomNumberGenerator.Create()) {
                var buff = new byte[2];
                rng.GetBytes(buff);

                var select = new byte[2] { 2, 16 }[(buff[0] + buff[1]) % 2];

                return new GameModel()
                {
                    type = select,
                    value = BitConverter.ToUInt16(buff),
                    answer = null
                };
            }
        }

        [HttpPost]
        public ActionResult<bool> Post([FromBody]GameModel answer)
        {
            try
            {
                if (answer.type != 2 && answer.type != 16)
                    throw new FormatException();

                ushort ans = 0;

                if (answer.type == 2)
                {
                    ans = ushort.Parse(answer.answer, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    ans = Convert.ToUInt16(answer.answer, 2);
                }

                return (answer.value == ans);
            }
            catch
            {
                return false;
            }
        }

    }
}
