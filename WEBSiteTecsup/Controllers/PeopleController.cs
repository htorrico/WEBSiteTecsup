using Newtonsoft.Json;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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


        public ActionResult Create()
        {
            return View();
        }

        // POST: Example/Create
        [HttpPost]
        public async Task<ActionResult> Create(PersonCreateModel model)
        {
            try
            {
                //Class a JSON
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                var urlBase = "https://localhost:44315";
                client.BaseAddress = new Uri(urlBase);
                var url = string.Concat(urlBase, "/Api", "/People", "/PostPerson");

                var response = client.PostAsync(url, content).Result;

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var result = await response.Content.ReadAsStringAsync();
                   
                    //Si sale algo con error, puedes enviar una alerta.
                    //var exito = JsonConvert.DeserializeObject<bool>(result);
                    
                }
             
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}