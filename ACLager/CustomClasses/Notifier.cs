using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ACLager.Interfaces;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class Notifier {

        private void Notify(object sender, NotificationEventArgs eventArgs) {
            if (eventArgs.ItemType.IngredientsForRecipe.Count != 0) {
                CreateWorkOrder(eventArgs.ItemType.UID);
            } else {
                Debug.WriteLine("SEND MAIL");
                //SendLowAmountMail(eventArgs.ItemType);
            }
        }

        public void Subscribe(INotifier notifierController) {
            notifierController.Notify += Notify;
        }

        private static void CreateWorkOrder(long itemTypeId) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                ItemType itemType = db.ItemTypeSet.Find(itemTypeId);

                WorkOrder workOrder = new WorkOrder {
                    BatchNumber = 11,
                    DueDate = DateTime.Now,
                    ShippingInfo = "AC",
                    Type = itemType.Department,
                    ItemType = itemType
                };
                db.WorkOrderSet.Add(workOrder);
                db.SaveChanges();
                
                foreach (Ingredient ingredient in itemType.IngredientsForRecipe) {
                    WorkOrderItem workOrderItem = new WorkOrderItem {
                        WorkOrder = workOrder,
                        Amount = ingredient.Amount,
                        ItemType = itemType,
                        Progress = 0
                    };
                    db.WorkOrderItemSet.Add(workOrderItem);
                }
                db.SaveChanges();
            }
        }

        private static void SendLowAmountMail(ItemType itemType) {
            //AUU smpt server "smtp.aau.dk", 587
            SmtpClient smtpClient = new SmtpClient("smtp.aau.dk", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential("id", "pw");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage();
            //Setting From, To and CC
            mail.From = new MailAddress("email", "Mig");
            mail.To.Add(new MailAddress("target email"));
            //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                mail.Subject = $"Lavt niveau af {itemType.Name}";
                mail.Body = $"Der er kun {itemType.Items.Sum(i => i.Amount)} {itemType.Name} tilbage, hvoraf {itemType.Items.Sum(i => i.Reserved)} er reserveret. \n " +
                            $"Bestil venligst flere. \n\n " +
                            $"Hilsen Systemet.";

                smtpClient.Send(mail);
            }
        }
    }
}