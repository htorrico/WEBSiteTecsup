using Newtonsoft.Json;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEBSiteTecsup.Models;


namespace WEBSiteTecsup.Controllers
{
    public class PeopleController : Controller
    {
        // GET: People
        public async Task<ActionResult> Index()
        {
            
            List<PersonModel> model = new List<PersonModel>();
            var client = new HttpClient();
            var urlBase = "https://localhost:44315";
            client.BaseAddress = new Uri(urlBase);
            var url = string.Concat(urlBase, "/Api", "/People", "/GetPeople");

            
            var response = client.GetAsync(url).Result;
            if (response.StatusCode== HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                //De JSON a Response
                var people = JsonConvert.DeserializeObject<List<PersonResponse>>(result);

                //De Response a Model
                model = (from c in people
                        select new PersonModel
                        {
                            FullName = string.Concat(c.FirstName, " ", c.LastName)
                        }).ToList();              
            }
            return View(model);
        }
    }
}