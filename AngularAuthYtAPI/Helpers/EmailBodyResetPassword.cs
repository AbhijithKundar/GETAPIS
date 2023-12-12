namespace AngularAuthYtAPI.Helpers
{
    public static class EmailBodyResetPassword
    {
        public static string EmailStringBodyResetPassword(string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<body style=""margin:0;padding:0;font-family: Arial, Helvetica, sans-serif;"">
<div style=""height: auto; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width:400px; padding:30px"">
<div>
<div>
<h1>Reset your password</h1>
<hr>
<p>You are recieving this email because you requested a password reset for your GET account.</p>
<p>Please tap the button below to choose a new password.</p>
<a href=""https://getui.azurewebsites.net/reset?email={email}&code={emailToken}"" target=""_blank="" style=""background:#0d6efc;
color:white; padding:10px; border:none; border-radius:4px; display:block; margin:0 auto; width:50%; text-align:center; text-decoration: none;"">
Reset password </a><br>

<p>Kind Regards,<br><br>
Team GET
</p>
</div>
</div>
</div>
</body>
</html>";
        }
    }
}
