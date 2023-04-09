using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controller
{
    public class GroupController : ControllerBase
    {
        List<string> _groups = new List<string>() { "DOC", "DOC_DevOps", "DOC_FE" };

        [HttpGet]
        public IActionResult GetAllAvailableGroups()
        {
            return Ok(_groups);
        }
    }
}
