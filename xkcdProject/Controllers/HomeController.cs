using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using xkcdProject.Models;

namespace xkcdProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string BaseDomain = "https://xkcd.com/";
        private const string BaseJson = "info.0.json";

        public Comic GetComic(int route)
        {
            int validacionInfity = 0;
            var cadena = "";
            using (WebClient cliente = new WebClient())
            {
                bool existsRoute = true;
                do
                {
                    string ruta = route == 0 ?
                    //Determinar la configuración inicial [https://xkcd.com/info.0.json]
                    $"{BaseDomain}{BaseJson}" :
                    //Obtener los numeros de cada comic desde 1 hasta el final
                    $"{BaseDomain}/{route}/{BaseJson}";
                    // Iniciamos la solicitud de datos al servidor
                    try
                    {
                        cadena = cliente.DownloadString(ruta);
                        existsRoute = true;
                    }
                    catch (WebException)
                    {
                        existsRoute = false;
                        route++;
                        validacionInfity++;
                    }
                    //Evitar un salto por fuera del limite
                    if (validacionInfity >= 4)
                    {
                        route = 1;
                    }
                } while (!existsRoute);
            }


            //En caso de que la cadena 
            if (cadena.Length > 0)
                return JsonConvert.DeserializeObject<Comic>(cadena);
            return new Comic();
        }

        public IActionResult Index(int? id, bool? preview)
        {
            //Siempre obtenemos la configuracion, para determinar el limite maximo
            var cmcSetings = GetComic(0);
            //Si id es null, asignale uno [1] por defecto
            var idValue = id ?? cmcSetings.Num;
            Console.WriteLine("Maximo = " + cmcSetings.Num);

            ViewBag.Position = GetComic(idValue);
            ViewBag.Settings = cmcSetings;
            //Si se cambio la ruta, hacer redirección, para que la url se conserveç
            var num = ((Comic)ViewBag.Position).Num;
            if (num != idValue)
            {
                //Validamos retroceso hacia valores not-found (ejemplo de 405 a 404, redireccionar a 403)
                num = (preview ?? false ? idValue - 1 : num);
                return Redirect("/Home/Index/" + num);
            }
            //Si sale bien, se retorna
            return View();
        }
    }
}
