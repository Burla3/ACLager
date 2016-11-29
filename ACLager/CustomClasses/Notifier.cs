using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ACLager.Interfaces;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class Notifier {

        private void Notify(object sender, NotificationEventArgs eventArgs) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                if (eventArgs.ItemType.IngredientsForRecipe != null) {
                    WorkOrder workOrder = new WorkOrder();
                    workOrder.BatchNumber = 11;
                    workOrder.DueDate = DateTime.Now;
                    workOrder.ShippingInfo = "AC";
                    workOrder.Type = "Production";
                    db.WorkOrderSet.Add(workOrder);

                    foreach (Ingredient ingredient in eventArgs.ItemType.IngredientsForRecipe) {
                        WorkOrderItem workOrderItem = new WorkOrderItem();
                        workOrderItem.Amount = ingredient.Amount;
                        workOrderItem.ItemType = eventArgs.ItemType;
                        workOrderItem.Progress = 0;
                        db.WorkOrderItemSet.Add(workOrderItem);
                    }   
                } else {
                    SmtpClient smtpClient = new SmtpClient("smtp.aau.dk", 587);

                    smtpClient.Credentials = new System.Net.NetworkCredential("id", "pw");
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;
                    MailMessage mail = new MailMessage();

                    //Setting From , To and CC
                    mail.From = new MailAddress("email", "Mig");
                    mail.To.Add(new MailAddress("target email"));
                    //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                    mail.Subject = $"Lavt nivaue af {eventArgs.ItemType}";
                    mail.Body = $"Der er kun {eventArgs.ItemType.Items.Sum(i => i.Amount)} {eventArgs.ItemType.Name} tilbage. \n Bestil venligst flere. \n Hilsen Systemet.";

                    smtpClient.Send(mail);
                }           
            }
        }

        public void Subscribe(INotifier notifierController) {
            notifierController.Notify += Notify;
        }
    }
}