using System;
using System.Collections.Generic;
using System.Text;
using Foe.Common;

namespace Foe.Client
{
    public static class FoeClientRegistration
    {
        public static bool IsUserRegistered()
        {
            FoeClientRegistryEntry reg = FoeClientRegistry.GetEntry("userid");
            if ((reg != null) && (reg.Value != null) && (reg.Value.Length > 0))
            {
                return true;
            }

            return false;
        }

        public static void SendRegistration()
        {
            // Check if user's email is available
            FoeClientRegistryEntry email = FoeClientRegistry.GetEntry("useremail");
            if ((email == null) || (email.Value == null) || (email.Value.Length < 3))
            {
                // Invalid user email
                return;
            }

            // Prepare message to send to server
            FoeMessage message = new FoeMessage();
            message.Add(new FoeMessageItem("UserEmail", email.Value));

            // Get SMTP server configurations
            SmtpServer server = null;
            try
            {
                server = FoeClientMessage.GetSmtpServer();
            }
            catch (Exception except)
            {
                throw new Exception("Invalid SMTP configurations.\r\n" + except.ToString());
            }

            // Send email
            try
            {
                string requestId = FoeClientRequest.GenerateId();
                FoeClientMessage.SendMessage(
                    server,
                    email.Value,
                    FoeClientRegistry.GetEntry("processoremail").Value,
                    SubjectGenerator.RequestSubject(RequestType.Registration, requestId, "Newbie"),
                    message);

                // Save request info
                FoeClientRequestItem req = new FoeClientRequestItem();
                req.Id = requestId;
                req.Type = "reg";
                req.DtRequested = DateTime.Now;
                FoeClientRequest.Add(req);
            }
            catch (Exception except)
            {
                throw new Exception("Error sending message.\r\n" + except.ToString());
            }
        }

    }
}
