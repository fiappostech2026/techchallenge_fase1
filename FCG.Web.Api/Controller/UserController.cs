using FCG.Domain.Entitie;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Controller;

public class UserController : ControllerBase
{
    // GET api/users/{id}
    [HttpGet("{id:guid}")]
    public ActionResult<User> GetById(Guid id)
    {
        User user = new User();

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    // POST api/users
    [HttpPost]
    public ActionResult<User> Create()
    {


        return Created();
    }

    // PUT api/users/{id}
    [HttpPut("{id:guid}")]
    public ActionResult<User> Update()
    {
        return Ok();
    }

    // DELETE api/users/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        return NoContent();
    }
}