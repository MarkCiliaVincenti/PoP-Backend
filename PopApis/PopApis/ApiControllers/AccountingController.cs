using Microsoft.AspNetCore.Mvc;
using PopLibrary;

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private SqlAdapter _sqlAdapter;

        public AccountingController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }
    }
}
