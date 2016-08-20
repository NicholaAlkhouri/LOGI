using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LOGI.Services
{
    public class PaymentOrderService
    {
         private ApplicationDbContext DbContext;
         private static string SECRET_KEY = ConfigurationManager.AppSettings["CybersourceSecret"];

         public PaymentOrderService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

         public int AddPaymentOrder(PaymentOrder order)
         {
             
             try
             {
                 DbContext.PaymentOrders.Add(order);

                 DbContext.SaveChanges();

                 return order.ID;
             }
             catch (Exception ex)
             {
                 return -1;
             }
         }



         public static String sign(IDictionary<string, string> paramsArray)
         {
             return sign(buildDataToSign(paramsArray), SECRET_KEY);
         }

         private static String sign(String data, String secretKey)
         {
             UTF8Encoding encoding = new System.Text.UTF8Encoding();
             byte[] keyByte = encoding.GetBytes(secretKey);

             HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
             byte[] messageBytes = encoding.GetBytes(data);
             return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
         }

         private static String buildDataToSign(IDictionary<string, string> paramsArray)
         {
             String[] signedFieldNames = paramsArray["signed_field_names"].Split(',');
             IList<string> dataToSign = new List<string>();

             foreach (String signedFieldName in signedFieldNames)
             {
                 dataToSign.Add(signedFieldName + "=" + paramsArray[signedFieldName]);
             }

             return commaSeparate(dataToSign);
         }

         private static String commaSeparate(IList<string> dataToSign)
         {
             return String.Join(",", dataToSign);
         }
    }
}