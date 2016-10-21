using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aDrumsLib;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.ADrums.Controllers
{
    [Route("api/[controller]")]
    public class TriggersController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<MidiTrigger> Get()
        {
            return DrumManager.Current.Triggers;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public MidiTrigger Get(int id)
        {
            return DrumManager.Current.Triggers
                .Where(x => (int)x.PinNumber == id)
                .FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]MidiTrigger value)
        {
            var i = DrumManager.Current.Triggers.FindIndex(x => x.PinNumber == value.PinNumber);
            if (1 > -1)
                DrumManager.Current.Triggers[i] = value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]MidiTrigger value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
