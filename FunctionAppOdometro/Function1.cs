using System;
using System.Net;
using System.Net.Mail;
using FunctionAppOdometro.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppOdometro
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("ejercicios", Connection = "MyConn")]string myQueueItem, ILogger log)
        {

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("a2016114304@estudiantes.upsa.edu.bo", "**********"),
                EnableSsl = true,
            };
         
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                var odometro = JsonConvert.DeserializeObject<Odometro>(myQueueItem);

                string mensaje ="Hola "+odometro.Name+" a la fecha "+odometro.DateTime+" ha caminado "+odometro.Step+" pasos.";

                smtpClient.Send("a2016114304@estudiantes.upsa.edu.bo", odometro.Email, "Resumen de pasos diarios", mensaje);
            }
            catch(Exception e)
            {
                log.LogError($"No fue posible enviar correo: {e.Message}");
            }

        }
    }
}
