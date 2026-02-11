using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.SD
{
    public static class EmailBody
    {

        public static string ResetPasswordTemplate(string firstName, string lastName, string role, string link)
        {
           return $@"
   <html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0;"">
                <table role=""presentation"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 30px; text-align: center;"">
                            <h1 style=""margin: 0; font-size: 24px;"">Password Reset Request</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 30px;"">
                            <h2 style=""margin: 0 0 15px 0; color: #333; font-size: 20px;"">Hello {firstName} {lastName},</h2>
                            <p style=""margin: 0 0 15px 0; color: #333;"">You have been invited to join the BBI Mileage Tracker as  <strong>{role}</strong>. You are receiving this request to reset your password for your Mileage Tracker account.</p>
                            <p style=""margin: 0 0 15px 0; color: #333;"">To reset your password, please click the button below:</p>
                            
                            <!-- Button -->
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
                                <tr>
                                    <td style=""text-align: center; padding: 20px 0;"">
                                        <a href=""{link}"" style=""display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 12px 30px; text-decoration: none; border-radius: 6px; font-weight: bold;"">Reset Password</a>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 15px 0 0 0; color: #6b7280; font-size: 12px;"">This password reset link will expire in 7 days or after it is used.</p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #6b7280; font-size: 12px;"">&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }
        public static string emailforReview(string fullName, string link)
        {
            return $@"
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0;"">
                <table role=""presentation"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 30px; text-align: center;"">
                            <h1 style=""margin: 0; font-size: 24px;"">Mileage Claim Review</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 30px;"">
                            <h2 style=""margin: 0 0 15px 0; color: #333; font-size: 20px;"">Hello,</h2>
                            <p style=""margin: 0 0 15px 0; color: #333;"">{fullName} has assigned a new mileage claim for your review.</p>
                            <p style=""margin: 0 0 15px 0; color: #333;"">Please click the button below to view and process the claim:</p>
                            
                            <!-- Button -->
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
                                <tr>
                                    <td style=""text-align: center; padding: 20px 0;"">
                                        <a href=""{link}"" style=""display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 12px 30px; text-decoration: none; border-radius: 6px; font-weight: bold;"">View Claim</a>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 15px 0 0 0; color: #6b7280; font-size: 12px;"">Please review this claim at your earliest convenience.</p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #6b7280; font-size: 12px;"">&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }

        public static string TeamLeadApproved(string fullName, string link)
        {
            return $@"
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0;"">
                <table role=""presentation"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 30px; text-align: center;"">
                            <h1 style=""margin: 0; font-size: 24px;"">Mileage Claim Review - Approved by Team Lead</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 30px;"">
                            <h2 style=""margin: 0 0 15px 0; color: #333; font-size: 20px;"">Hello,</h2>
                            <p style=""margin: 0 0 15px 0; color: #333;"">{fullName} has approved a mileage claim and needs your final approval to be processed.</p>
                            <p style=""margin: 0 0 15px 0; color: #333;"">Please click the button below to view and process the claim:</p>
                            
                            <!-- Button -->
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
                                <tr>
                                    <td style=""text-align: center; padding: 20px 0;"">
                                        <a href=""{link}"" style=""display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 12px 30px; text-decoration: none; border-radius: 6px; font-weight: bold;"">View Claim</a>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 15px 0 0 0; color: #6b7280; font-size: 12px;"">Please review this claim at your earliest convenience.</p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #6b7280; font-size: 12px;"">&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }

        public static string ClaimFullyApproved(
            string fullName,
            decimal amount,
            string link,
            string? financeComment = null
        )
        {
            var financeCommentSection = string.IsNullOrWhiteSpace(financeComment)
                ? string.Empty
                : $@"
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse; margin-top: 20px;"">
                                <tr>
                                    <td style=""padding: 15px; background-color: #ecfeff; border-left: 4px solid #06b6d4; border-radius: 4px;"">
                                        <p style=""margin: 0 0 8px 0; font-weight: bold; color: #333;"">Finance Comments:</p>
                                        <p style=""margin: 0; color: #333;"">{financeComment}</p>
                                    </td>
                                </tr>
                            </table>";

            return $@"
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0;"">
                <table role=""presentation"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 30px; text-align: center;"">
                            <h1 style=""margin: 0; font-size: 24px;"">Mileage Claim Approved</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 30px;"">
                            <h2 style=""margin: 0 0 15px 0; color: #333; font-size: 20px;"">Hello {fullName},</h2>
                            <p style=""margin: 0 0 15px 0; color: #333;"">Your mileage claim has been <strong>approved by both your Team Lead and the Finance team</strong>.</p>
                            <p style=""margin: 0 0 15px 0; color: #333;"">You will be reimbursed the following amount:</p>
                            
                            <!-- Amount -->
                            <div style=""font-size: 20px; font-weight: bold; color: #059669; margin: 15px 0; text-align: center;"">
                                ${amount:N2}
                            </div>
                            
                            {financeCommentSection}
                            
                            <p style=""margin: 20px 0 15px 0; color: #333;"">You can view the approved claim details by clicking the button below:</p>
                            
                            <!-- Button -->
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
                                <tr>
                                    <td style=""text-align: center; padding: 20px 0;"">
                                        <a href=""{link}"" style=""display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); background-color: #667eea; color: #ffffff; padding: 12px 30px; text-decoration: none; border-radius: 6px; font-weight: bold;"">View Claim</a>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 15px 0 0 0; color: #6b7280; font-size: 12px;"">If you have any questions regarding reimbursement, please contact the finance team.</p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #6b7280; font-size: 12px;"">&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }

        public static string ClaimRejected(
            string fullName,
            string link,
            string? financeComment = null
        )
        {
            var commentSection = string.IsNullOrWhiteSpace(financeComment)
                ? string.Empty
                : $@"
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse; margin-top: 20px;"">
                                <tr>
                                    <td style=""padding: 15px; background-color: #fff1f2; border-left: 4px solid #ef4444; border-radius: 4px;"">
                                        <p style=""margin: 0 0 8px 0; font-weight: bold; color: #333;"">Reason / Comments:</p>
                                        <p style=""margin: 0; color: #333;"">{financeComment}</p>
                                    </td>
                                </tr>
                            </table>";

            return $@"
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0;"">
                <table role=""presentation"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); background-color: #ef4444; color: #ffffff; padding: 30px; text-align: center;"">
                            <h1 style=""margin: 0; font-size: 24px;"">Mileage Claim Rejected</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 30px;"">
                            <h2 style=""margin: 0 0 15px 0; color: #333; font-size: 20px;"">Hello {fullName},</h2>
                            <p style=""margin: 0 0 15px 0; color: #333;"">Unfortunately, your mileage claim has been <strong>reviewed and rejected</strong>.</p>
                            <p style=""margin: 0 0 15px 0; color: #333;"">You can review the claim details using the button below. If applicable, please update and resubmit the claim.</p>
                            
                            {commentSection}
                            
                            <!-- Button -->
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse;"">
                                <tr>
                                    <td style=""text-align: center; padding: 20px 0;"">
                                        <a href=""{link}"" style=""display: inline-block; background-color: #6b7280; color: #ffffff; padding: 12px 30px; text-decoration: none; border-radius: 6px; font-weight: bold;"">View Claim</a>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 15px 0 0 0; color: #6b7280; font-size: 12px;"">If you have questions, please contact your Team Lead or the Finance team.</p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f9fafb; padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #6b7280; font-size: 12px;"">&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }


    }
}
