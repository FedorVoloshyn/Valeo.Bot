using System.Text;

namespace ValeoBot.Configuration
{
    public class SMTPConnection
    {
        public string Server { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string CompanyName { get; set; }
        public int TimeOut { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Server:").Append(Server.ToString()).Append("\n");
            sb.Append("Recipient:").Append(Recipient.ToString()).Append("\n");
            sb.Append("Subject:").Append(Subject.ToString()).Append("\n");
            sb.Append("UserName:").Append(UserName.ToString()).Append("\n");
            sb.Append("Password:").Append(Password.ToString()).Append("\n");
            sb.Append("Port:").Append(Port.ToString()).Append("\n");
            sb.Append("UseSSL:").Append(UseSSL.ToString()).Append("\n");
            sb.Append("CompanyName:").Append(CompanyName.ToString()).Append("\n");
            sb.Append("TimeOut:").Append(TimeOut.ToString()).Append("\n");

            return sb.ToString();
        }
    }
}