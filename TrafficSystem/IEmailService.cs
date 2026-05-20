//Interface for sending emails

public interface IEmailService
{
    //Sends an email with subject and message
    void SendMail(string to, string subject, string message);
} 