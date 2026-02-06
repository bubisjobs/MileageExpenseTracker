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
        <style>
            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
            .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
            .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
            .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
            .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Password Reset Request</h1>
            </div>
            <div class='content'>
                <h2>Hello {firstName} {lastName},</h2>
                <p>You have been invited to join the BBI Mileage Tracker as a <strong>{role}</strong>. You are receiving this request to reset your password for your Mileage Tracker account.</p>
                <p>To reset your password, please click the button below:</p>
              

                <div style='text-align: center;'>
                    <a href='{link}' class='button' style='color: white;'>Reset Password</a>
                </div>
                <p><small>This password reset link will expire in 7 days or after it is used.</small></p>
            </div>
            <div class='footer'>
                <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>
";
        }
        public static string emailforReview(string fullName, string link)
        {
            return $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Mileage Claim Review</h1>
        </div>
        <div class='content'>
            <h2>Hello,</h2>
            <p>{fullName} has assigned a new mileage claim for your review.</p>
            <p>Please click the button below to view and process the claim:</p>

            <div style='text-align: center;'>
                <a href='{link}' class='button' style='color: white;'>View Claim</a>
            </div>

            <p><small>Please review this claim at your earliest convenience.</small></p>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";
        }
        public static string TeamLeadApproved(string fullName, string link)
        {
            return $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Mileage Claim Review - Approved by Team Lead</h1>
        </div>
        <div class='content'>
            <h2>Hello,</h2>
            <p>{fullName} has approved a mileage claim and needs your final approval to be processed.</p>
            <p>Please click the button below to view and process the claim:</p>

            <div style='text-align: center;'>
                <a href='{link}' class='button' style='color: white;'>View Claim</a>
            </div>

            <p><small>Please review this claim at your earliest convenience.</small></p>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
        </div>
    </div>
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
            <div style='margin-top: 20px; padding: 15px; background: #ecfeff; border-left: 4px solid #06b6d4; border-radius: 4px;'>
                <strong>Finance Comments:</strong>
                <p style='margin: 8px 0 0;'>{financeComment}</p>
            </div>";

            return $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .amount {{ font-size: 1.25rem; font-weight: 700; color: #059669; margin: 15px 0; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Mileage Claim Approved</h1>
        </div>

        <div class='content'>
            <h2>Hello {fullName},</h2>

            <p>Your mileage claim has been <strong>approved by both your Team Lead and the Finance team</strong>.</p>

            <p>You will be reimbursed the following amount:</p>

            <div class='amount'>
                ${amount:N2}
            </div>

            {financeCommentSection}

            <p style='margin-top: 20px;'>
                You can view the approved claim details by clicking the button below:
            </p>

            <div style='text-align: center;'>
                <a href='{link}' class='button' style='color: white;'>View Claim</a>
            </div>

            <p><small>If you have any questions regarding reimbursement, please contact the finance team.</small></p>
        </div>

        <div class='footer'>
            <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
        </div>
    </div>
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
            <div style='margin-top: 20px; padding: 15px; background: #fff1f2; border-left: 4px solid #ef4444; border-radius: 4px;'>
                <strong>Reason / Comments:</strong>
                <p style='margin: 8px 0 0;'>{financeComment}</p>
            </div>";

            return $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background: #6b7280; color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Mileage Claim Rejected</h1>
        </div>

        <div class='content'>
            <h2>Hello {fullName},</h2>

            <p>
                Unfortunately, your mileage claim has been <strong>reviewed and rejected</strong>.
            </p>

            <p>
                You can review the claim details using the button below. If applicable,
                please update and resubmit the claim.
            </p>

            {commentSection}

            <div style='text-align: center;'>
                <a href='{link}' class='button' style='color: white;'>View Claim</a>
            </div>

            <p><small>If you have questions, please contact your Team Lead or the Finance team.</small></p>
        </div>

        <div class='footer'>
            <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";
        }



    }
}
