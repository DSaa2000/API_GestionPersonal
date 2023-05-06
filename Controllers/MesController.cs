using API.DataBase;
using API.Models.UtilidadGeneral;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesController : ControllerBase
    {
        private SitioDB db;
        public MesController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpGet]
        [Route("List")]
        public async Task<List<MesDB>> Listar()
        {
            List<MesDB> list = await db.Meses.ToListAsync();

            return list;

        }

    }
}
