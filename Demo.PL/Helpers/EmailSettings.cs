using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public class EmailSettings
    {
        public static void sendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);


                client.EnableSsl = true;
          client.Credentials=  new NetworkCredential("mohamedb.555dr@gmail.com", "hvxsqfgdfajjtava");
            client.Send("mohamedb.555dr@gmail.com", email.Recipiens, email.Subject, email.Body);

        }
    }
}
